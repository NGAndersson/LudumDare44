using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameContext : MonoBehaviour
{
    public static bool isGamePaused = true;

    public List<GameObject> gameGlobalObjects;
    private MenuEvents menuEvents;
    private PlayerController playerController;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        foreach (GameObject gameObject in gameGlobalObjects)
        {
            DontDestroyOnLoad(Instantiate(gameObject));
        }
    }

    private void OnEnable()
    {
        menuEvents = Utilities.Scene.findExactlyOne<MenuEvents>();
        playerController = Utilities.Scene.findExactlyOne<PlayerController>();
    }

    private void Start()
    {
        if (Application.isEditor)
        {
            TogglePause();
            StartNewGame();
        }
    }

    public void TogglePause()
    {
        isGamePaused = !isGamePaused;
        Time.timeScale = (isGamePaused) ? 0 : 1;
    }

    public void StartNewGame()
    {
        print("Start new game");
    }
}
