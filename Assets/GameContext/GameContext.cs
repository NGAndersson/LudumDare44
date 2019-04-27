using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameContext : MonoBehaviour
{
    public static bool isGamePaused = true;

    public List<GameObject> gameGlobalObjects;

    void Awake()
    {
        DontDestroyOnLoad(this);
        foreach (GameObject gameObject in gameGlobalObjects)
        {
            DontDestroyOnLoad(Instantiate(gameObject));
        }
    }

    void Start()
    {
        if (Application.isEditor)
        {
            TogglePause();
            StartNewGameDelayed();
        }
    }

    public void TogglePause()
    {
        isGamePaused = !isGamePaused;
        Time.timeScale = (isGamePaused) ? 0 : 1;
    }

    public void StartNewGameDelayed()
    {
        StartCoroutine("StartNewGame");
    }

    private IEnumerator StartNewGame()
    {
        EnemyWaves enemyWaves = Utilities.Scene.findExactlyOne<EnemyWaves>();
        yield return new WaitForSeconds(2f);
        enemyWaves.Reset();
        enemyWaves.ForceNextWave();
    }
}
