using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaves : MonoBehaviour
{
    public EnemyWave enemyWavePrefab;

    public GameObject enemyTypeOne; // TODO Change name?
    public GameObject enemyTypeTwo;
    public GameObject enemyTypeThree;

    int enemyWaveCounter = 1;
    EnemyWave currentEnemyWave;
    PlayerController playerController;

    public void Reset()
    {
        enemyWaveCounter = 1;
    }

    private void OnEnable()
    {
        playerController = Utilities.Scene.findExactlyOne<PlayerController>();
    }

    private bool TimeForNextWave()
    {
        return false;
        if(currentEnemyWave != null && currentEnemyWave.GetAliveEnemies() == 0)
        {
            return true;
        }
        return false;
    }

    void FixedUpdate()
    {
        if(TimeForNextWave())
        {
            NextWave();
        }
    }

    public void ForceNextWave()
    {
        NextWave();
    }

    void NextWave()
    {
        if (currentEnemyWave) { Destroy(currentEnemyWave); }
        currentEnemyWave = BuildNextWave(enemyWaveCounter);
        ++enemyWaveCounter;
    }

    // TODO This function can be moved out of this class.
    private EnemyWave BuildNextWave(int enemyWaveCounter)
    {
        EnemyWave enemyWave = Instantiate(enemyWavePrefab);
        // TODO fins spawn position for enememies
        if(enemyWaveCounter == 1)
        {
            enemyWave.enemeies.Add(BuildEnemyList(enemyTypeOne, 10, playerController));
        }
        else
        {
            // TODO This is just examples
            enemyWave.enemeies.Add(BuildEnemyList(enemyTypeOne, (10 * enemyWaveCounter / 2), playerController));
           // enemyWave.enemeies.Add(BuildEnemyList(enemyTypeTwo, 10));
           // enemyWave.enemeies.Add(BuildEnemyList(enemyTypeThree, 10));

        }
        return enemyWave;
    }

    private List<GameObject> BuildEnemyList(GameObject enemyPrefab, int amount, PlayerController playerController)
    {
        List<GameObject> enemyList = new List<GameObject>();
        for(int i = 0; i < amount; ++i)
        {
           GameObject enemyObject = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
           Enemy enemy = enemyObject.GetComponent<Enemy>();
           enemy.Initialize(playerController, playerController.GetComponentInParent<Rigidbody>());
        }
        return enemyList;
    }
}
