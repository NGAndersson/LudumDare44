using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEvents : MonoBehaviour
{
    public UnityEngine.UI.Text topText;

    GameContext gameContext;

    AudioSource crowdSource;
    public AudioClip crowdClap;
    public AudioClip crowdCheer;

    void Start()
    {
        gameContext = Utilities.Scene.findExactlyOne<GameContext>();
        crowdSource = GameObject.Find("Audio_Crowd").GetComponent<AudioSource>();
        if(Application.isEditor)
        {
            StartCoroutine("DelayedStart");
        }
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.1f);
        EventPlay();
    }

    public void EventPlay()
    {
        topText.text = "Restart";
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

    public void ToggleMenuShow()
    {
        gameContext.TogglePause(true);
        crowdSource.clip = crowdCheer;
        crowdSource.Play();
        transform.parent.gameObject.SetActive(GameContext.isGamePaused);
    }

    public void ToggleMenuHide()
    {
        gameContext.TogglePause(false);
        crowdSource.clip = crowdClap;
        crowdSource.Play();
        transform.parent.gameObject.SetActive(GameContext.isGamePaused);
    }

    public void ToggleMenu()
    {
        gameContext.TogglePause(!GameContext.isGamePaused);
        transform.parent.gameObject.SetActive(GameContext.isGamePaused);
    }
}
