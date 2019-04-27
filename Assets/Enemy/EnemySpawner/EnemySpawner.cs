using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject origin;
    public GameObject direction;

    List<Enemy> enemyList = new List<Enemy>();

    void Start()
    {
        origin.SetActive(false);
        direction.SetActive(false);
        GetComponent<MeshRenderer>().enabled = false;
    }

    public GameObject QueueEnemy(GameObject enemyPrefab, EnemySpawner enemySpawner)
    {
        GameObject enemyGameObject = Instantiate(enemyPrefab, GetLastEmptyPosition(), Quaternion.identity);
        Enemy enemy = enemyGameObject.GetComponent<Enemy>();
        enemy.SetTarget(direction.transform);
        enemyList.Add(enemy);
        return enemyGameObject;
    }

    void Update()
    {
        for (int i = 0; i< enemyList.Count; )
        {
            float distance = (enemyList[i].transform.position - direction.transform.position).magnitude;
            if (distance <= 2f)
            {
                enemyList[i].ReleaseFromSpawner();
                enemyList.RemoveAt(i);
            }
            else
            {
                ++i;
            }
        }
    }
    
    Vector3 GetLastEmptyPosition()
    {
        float sizeOfEnemy = 3; // TODO
        Vector3 direction = (origin.transform.position - transform.position).normalized;
        Vector3 spawnPosition = origin.transform.position + (direction * sizeOfEnemy * enemyList.Count);
        return spawnPosition;
    }

    public void Reset()
    {
        enemyList = new List<Enemy>();
    }
}
