using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    [HideInInspector]
    public List<List<GameObject>> enemeies = new List<List<GameObject>>();

    void Start()
    {
       
    }

    void Update()
    {
    }

    public int GetAliveEnemies()
    {
        int counter = 0;
        foreach(List<GameObject> enemyList in enemeies)
        {
            counter += enemyList.Count;
        }
        return counter;
    }
}
