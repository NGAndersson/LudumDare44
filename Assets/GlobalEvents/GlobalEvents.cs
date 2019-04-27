using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEvents : MonoBehaviour
{
    MenuEvents menuEvents;

    void Start()
    {
        menuEvents = Utilities.Scene.findExactlyOne<MenuEvents>();
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            menuEvents.ToggleMenu();
        }
    }
}
