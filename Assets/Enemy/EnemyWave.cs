using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    [HideInInspector]
    public List<List<GameObject>> enemeies = new List<List<GameObject>>();

    void Update()
    {
    }

    void OnDestroy()
    {
        foreach (List<GameObject> enemyList in enemeies)
        {
            foreach (GameObject enemy in enemyList)
            {
                Destroy(enemy);
            }
        }
    }

    public int GetAliveEnemies()
    {
        int counter = 0;
        foreach(List<GameObject> enemyList in enemeies)
        {
            foreach (GameObject enemy in enemyList)
            {
                counter += (enemy.activeSelf) ? 1 : 0;
            }
        }
        return counter;
    }
}
