using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    MenuManager menuManager;
    bool gameEnded;

    private void Start()
    {
        menuManager = FindObjectOfType<MenuManager>();
        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && menuManager != null && !gameEnded)
        {
            menuManager.GamePausing();
        }
    }

    public void LostGame()
    {
        gameEnded = true;
        FindObjectOfType<MenuManager>().ShowLostScreen();
    }
}
