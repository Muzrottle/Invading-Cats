using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject lostMenu;
    bool isPaused = false;

    public void OnButtonEnter(GameObject pointer)
    {
        pointer.SetActive(true);
    }

    public void OnButtonExit(GameObject pointer)
    {
        pointer.SetActive(false);
    }

    void TurnOffOtherUI()
    {
        foreach (Transform canvasElement in canvas.transform)
        {
            canvasElement.gameObject.SetActive(false);
        }
    }

    void TurnOnOtherUI()
    {
        foreach (Transform canvasElement in canvas.transform)
        {
            canvasElement.gameObject.SetActive(true);
        }
    }

    public void GamePausing()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
        }
    }

    public void ShowLostScreen()
    {
        TurnOffOtherUI();
        lostMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LeaveGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
