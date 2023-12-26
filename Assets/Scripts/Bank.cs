using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class Bank : MonoBehaviour
{
    [SerializeField] int startBalance = 150;
    [SerializeField] int currentBalance;
    public int CurrentBalance { get { return currentBalance; } }

    [SerializeField] TextMeshProUGUI coinText;

    void Awake()
    {
        currentBalance = startBalance;
        UpdateBalanceText();
    }

    public void Deposit(int amount)
    {
        currentBalance += Mathf.Abs(amount);
        UpdateBalanceText();
    }

    public void Withdraw(int amount)
    {
        currentBalance -= Mathf.Abs(amount);
        UpdateBalanceText();

        if (currentBalance < 0)
        {
            ReloadScene();
        }
    }

    private void UpdateBalanceText()
    {
        coinText.text = $"{currentBalance}";
    }

    void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
