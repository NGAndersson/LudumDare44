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
        StartCoroutine("DelayedStart");
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.1f);
        transform.parent.gameObject.SetActive(false);
    }

    public void EventPlay()
    {
        EventToggleMenu();
        gameContext.StartNewGameDelayed();
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
