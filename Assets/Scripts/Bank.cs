using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using DG.Tweening;

public class Bank : MonoBehaviour
{
    [SerializeField] int startBalance = 150;
    [SerializeField] int currentBalance;
    public int CurrentBalance { get { return currentBalance; } }

    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] GameObject BalanceChangeParent;
    [SerializeField] GameObject currentChange;

    void Awake()
    {
        currentBalance = startBalance;
        coinText.text = $"{currentBalance}";
    }

    public void Deposit(int amount)
    {
        int previousBalance = currentBalance;
        currentBalance += Mathf.Abs(amount);
        UpdateBalanceText(previousBalance);
        ShowBalanceChange(true, amount);
    }

    public void Withdraw(int amount)
    {
        int previousBalance = currentBalance;
        currentBalance -= Mathf.Abs(amount);
        UpdateBalanceText(previousBalance);
        ShowBalanceChange(false, amount);

        if (currentBalance < 0)
        {
            FindObjectOfType<GameManager>().LostGame();
        }
    }

    private void UpdateBalanceText(int previousBalance)
    {
        DOTween.To(() => previousBalance, x => previousBalance = x, currentBalance, 2f)
            .SetEase(Ease.Linear) // Optional: Set the easing function
            .OnUpdate(() => coinText.text = previousBalance.ToString());
    }

    void ShowBalanceChange(bool isIncome, int amount)
    {
        GameObject myBalanceChange = Instantiate(currentChange, BalanceChangeParent.transform);
        Color changeColor;

        if (isIncome)
        {
            changeColor = new Color(0.5325002f, 0.9058824f, 0.1921568f);
            myBalanceChange.GetComponentInChildren<TextMeshProUGUI>().text = "+" + amount.ToString();
            myBalanceChange.GetComponentInChildren<TextMeshProUGUI>().color = changeColor;
        }
        else
        {
            changeColor = new Color(0.9056604f, 0.1913848f, 0.1913848f);
            myBalanceChange.GetComponentInChildren<TextMeshProUGUI>().text = "-" + amount.ToString();
            myBalanceChange.GetComponentInChildren<TextMeshProUGUI>().color = changeColor;
        }

        RectTransform myRect = myBalanceChange.GetComponent<RectTransform>();
        Vector3 nextPos = new Vector3(myRect.position.x, myRect.position.y + 40, myRect.position.z);
        myRect.DOMove(nextPos, 0.5f)
            .OnComplete(() =>
                Destroy(myBalanceChange)
            );
    }

    //void ReloadScene()
    //{
    //    Scene currentScene = SceneManager.GetActiveScene();
    //    SceneManager.LoadScene(currentScene.buildIndex);
    //}
}
