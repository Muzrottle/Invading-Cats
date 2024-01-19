using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] int cost = 75;
    [SerializeField] int sell = 50;
    [SerializeField] float buildSecForEachPart = 0.2f;

    public bool CreateTower(Tower tower, Vector3 position, Transform parent)
    {
        Bank bank = FindObjectOfType<Bank>();

        if (bank == null)
            return false;

        if (bank.CurrentBalance >= cost) 
        {
            GameObject dog = Instantiate(tower.gameObject, position, Quaternion.identity, parent);
            Tower dogTower = dog.GetComponent<Tower>();

            bank.Withdraw(cost);
            dogTower.StartCoroutine(BuildDog(dog));

            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator BuildDog(GameObject dog)
    {
        foreach (Transform child in dog.transform)
        {
            child.gameObject.SetActive(false);
        }

        int i = 0;

        foreach (Transform child in dog.transform)
        {
            child.gameObject.SetActive(true);
            Debug.Log(i);
            i++;
            yield return new WaitForSeconds(buildSecForEachPart);
        }

        dog.GetComponent<TargetLocator>().enabled = true;
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
