using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEvents : MonoBehaviour
{
    GameContext gameContext;

    void Start()
    {
        gameContext = Utilities.Scene.findExactlyOne<GameContext>();
    }

    public void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            gameContext.toggleMenu();
        }
    }

    public void EventPlay()
    {
        gameContext.startNewGame();
    }

    public void EventOptions()
    {
        print("Options code goes here.");
    }

    public void EventQuit()
    {
        Application.Quit();
    }
}
