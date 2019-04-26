using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTriggers : MonoBehaviour
{
    public void Play()
    {
        StartCoroutine(LoadGameScene());
    }

    public void Options()
    {
        print("Options code goes here.");
    }

    public void Exit()
    {
        Application.Quit();
    }

    IEnumerator LoadGameScene()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("SampleScene");
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }
}
