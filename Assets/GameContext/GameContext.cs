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

    public void TogglePause(bool pause = true)
    {
        isGamePaused = pause;
        Time.timeScale = (isGamePaused) ? 0 : 1;
    }

    public void StartNewGame()
    {
        StartCoroutine("StartNewGameInternal");
    }

    private IEnumerator StartNewGameInternal()
    {
        TogglePause(false);
        yield return new WaitForSeconds(0.1f);
        Utilities.Scene.findExactlyOne<EnemySpawnerController>().Reset();
        GameObject.FindWithTag("Player").GetComponentInChildren<PlayerController>().Reset();
        MenuEvents menuEvents = Utilities.Scene.findExactlyOne<MenuEvents>();
        menuEvents.ToggleMenuHide();
        EnemyWaves enemyWaves = Utilities.Scene.findExactlyOne<EnemyWaves>();
        enemyWaves.Reset();
        enemyWaves.ForceNextWave();
    }
}
