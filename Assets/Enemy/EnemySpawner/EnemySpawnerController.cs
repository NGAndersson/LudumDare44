using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemySpawnerController : MonoBehaviour
{
    EnemySpawner[] enemySpawners;

    void OnEnable()
    {
        enemySpawners = FindObjectsOfType<EnemySpawner>();
        Assert.AreNotEqual(0, enemySpawners.Length);
    }

    public EnemySpawner GetEnemmySpawner(int index)
    {
        return enemySpawners[index];
    }

    public void Reset()
    {
        foreach(EnemySpawner enemySpawner in enemySpawners)
        {
            enemySpawner.Reset();
        }
    }
}
