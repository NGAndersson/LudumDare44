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

    void OnDestroy()
    {
        print("OnDestroy: " + GetAliveEnemies());
        foreach (List<GameObject> enemyList in enemeies)
        {
            foreach (GameObject enemy in enemyList)
            {
        print("OnDestroy12");
                Destroy(enemy);
            }
        }
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
