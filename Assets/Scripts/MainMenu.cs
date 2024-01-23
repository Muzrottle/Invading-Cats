using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        if (Application.isPlaying)
        {
            SceneManager.LoadScene(1);
            DOTween.KillAll();
        }
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
