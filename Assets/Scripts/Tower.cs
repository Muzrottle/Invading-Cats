using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] int cost = 75;
    [SerializeField] int sell = 50;

    public bool CreateTower(Tower tower, Vector3 position, Transform parent)
    {
        Bank bank = FindObjectOfType<Bank>();

        if (bank == null)
            return false;

        if (bank.CurrentBalance >= cost) 
        {
            Instantiate(tower.gameObject, position, Quaternion.identity, parent);
            bank.Withdraw(cost);
            
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool DestroyTower(GameObject tower)
    {
        Bank bank = FindObjectOfType<Bank>();

        if (bank == null)
        {
            return true;
        }
        else
        {
            Destroy(tower.gameObject);
            bank.Deposit(sell);
            return false;
        }
    }
}
