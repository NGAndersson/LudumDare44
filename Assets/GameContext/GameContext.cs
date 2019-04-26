using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameContext : MonoBehaviour
{
    public List<GameObject> gameGlobalObjects;

    bool gamePaused = false;

    private void Awake()
    {
        foreach(GameObject gameObject in gameGlobalObjects)
        {
            Instantiate(gameObject);
        }
    }

    void togglePause()
    {
        gamePaused = !gamePaused;
        Time.timeScale = (gamePaused) ? 0 : 1;
    }

    public void toggleMenu()
    {
        togglePause();
    }

    public void startNewGame()
    {

    }
}
