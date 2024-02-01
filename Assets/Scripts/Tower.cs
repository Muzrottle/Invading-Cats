using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] int cost = 75;
    public int Cost { get { return cost; } }
    [SerializeField] int sell = 50;
    public int Sell { get { return sell; } }
    [SerializeField] int upgrade = 25;
    public int Upgrade { get { return upgrade; } }
    [SerializeField] float buildSecForEachPart = 0.2f;
    [SerializeField] float atkSpeedIncRate = 0.15f;
    [SerializeField] float atkRangeIncRate = 5f;
    public ParticleSystem myParticleSystem;
    [SerializeField] GameObject stars;

    int powerLevel = 0;

    public bool CreateTower(Tower tower, Tile tile)
    {
        Bank bank = FindObjectOfType<Bank>();

        if (bank == null)
            return false;

        if (bank.CurrentBalance >= cost) 
        {
            GameObject buildingTower = Instantiate(tower.gameObject, tile.transform.position, Quaternion.identity, tile.transform);
            tile.SetTilePlacement(true, buildingTower);

            Tower dogTower = buildingTower.GetComponent<Tower>();

            bank.Withdraw(cost);
            dogTower.StartCoroutine(BuildDog(buildingTower));

            return true;
        }
        else
        {
            tile.SetTilePlacement(false);
            return false;
        }
    }

    IEnumerator BuildDog(GameObject buildingTower)
    {
        foreach (Transform child in buildingTower.GetComponentInChildren<TargetLocator>().transform)
        {
            child.gameObject.SetActive(false);
        }

        int i = 0;

        foreach (Transform child in buildingTower.GetComponentInChildren<TargetLocator>().transform)
        {
            child.gameObject.SetActive(true);
            i++;
            yield return new WaitForSeconds(buildSecForEachPart);
        }

        buildingTower.GetComponentInChildren<TargetLocator>().enabled = true;
    }

    public bool DestroyTower(GameObject tower, Tile tile)
    {
        Bank bank = FindObjectOfType<Bank>();

        if (bank == null)
        {
            return true;
        }
        else
        {
            tile.SetTilePlacement(false, tower.gameObject);
            Destroy(tower.gameObject);
            bank.Deposit(sell);
            return false;
        }
    }

    public bool TowerPowerUp()
    {
        Bank bank = FindObjectOfType<Bank>();

        if (powerLevel == 3 || bank == null)
        {
            return false;
        }
        else
        {
            bank.Withdraw(upgrade);
        }

        powerLevel += 1;
        int currentLevel = powerLevel;

        foreach (Transform star in stars.transform)
        {
            star.gameObject.SetActive(true);
            currentLevel--;

            if (currentLevel == 0)
            {
                break;
            }
        }

        ParticleSystem.MainModule mainModule= myParticleSystem.main;
        mainModule.simulationSpeed += atkSpeedIncRate;
        mainModule.startLifetime = mainModule.startLifetime.constant + atkSpeedIncRate;
        gameObject.GetComponentInChildren<TargetLocator>().range += atkRangeIncRate;

        return true;
    }
}
