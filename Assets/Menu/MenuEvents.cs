using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEvents : MonoBehaviour
{
    GameContext gameContext;

    void OnEnable()
    {
        gameContext = Utilities.Scene.findExactlyOne<GameContext>();
    }

    private void Start()
    {
        if (Application.isEditor)
        {
            transform.parent.gameObject.SetActive(false);
        }
    }

    public void EventPlay()
    {
        gameContext.StartNewGame();
    }

    public void EventOptions()
    {
        print("Options code goes here.");
    }

    public void EventCredits()
    {
        print("Credits.");
    }

    public void EventQuit()
    {
        Application.Quit();
    }

    public void EventToggleMenu()
    {
        gameContext.TogglePause();
        transform.parent.gameObject.SetActive(GameContext.isGamePaused);
    }
}
