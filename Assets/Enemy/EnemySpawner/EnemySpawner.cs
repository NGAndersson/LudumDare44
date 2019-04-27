using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject origin;
    public GameObject direction;

    Queue<Enemy> queuedEnemeis = new Queue<Enemy>();
    Enemy currentEnemy = null;

    public GameObject QueueEnemy(GameObject enemyPrefab, PlayerController playerController, EnemySpawner enemySpawner)
    {
        GameObject enemyGameObject = Instantiate(enemyPrefab, GetLastEmptyPosition(), Quaternion.identity);
        Enemy enemy = enemyGameObject.GetComponent<Enemy>();
        enemy.Initialize(playerController, playerController.GetComponentInParent<Rigidbody>()); // TODO prefetch Rigidbody
        enemy.SetTarget(direction.transform);
        queuedEnemeis.Enqueue(enemy);
        return enemyGameObject;
    }

    void FixedUpdate()
    {
        if (queuedEnemeis.Count == 0) // If there's nothing queued, reset current and leave.
        {
            currentEnemy = null;
            return;
        }

        if (currentEnemy == null) // If There's no current emeny, peek the first one.
        {
            currentEnemy = queuedEnemeis.Peek();
        }

        if((currentEnemy.transform.position - direction.transform.position).magnitude <= 1f)
        {
            currentEnemy = queuedEnemeis.Dequeue();
            currentEnemy.ReleaseFromSpawner();
        }
    }
    
    Vector3 GetLastEmptyPosition()
    {
        float sizeOfEnemy = 3; // TODO
        Vector3 direction = (origin.transform.position - transform.position).normalized;
        Vector3 spawnPosition = origin.transform.position + (direction * sizeOfEnemy * queuedEnemeis.Count);
        return spawnPosition;
    }
}
