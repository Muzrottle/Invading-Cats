using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int startHP = 3;
    int maxHP;
    [SerializeField] int currentHP = 0;

    Enemy enemy;
    HealthBar healthBar;
    WaveHandler waveHandler;

    private void OnEnable()
    {
        currentHP = maxHP;
        
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHP);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        ProcessHit();
    }

    private void ProcessHit()
    {
        currentHP--;
        healthBar.SetHealth(currentHP);

        Debug.Log("I'm Hit!");

        if (currentHP == 0)
        {
            EnemyDied();
        }
    }

    private void EnemyDied()
    {
        Debug.Log("I died!");

        enemy.RewardGold();
        gameObject.SetActive(false);
        waveHandler.LowerAliveCount();
    }

    public void SetNewHealth(bool isBoss)
    {
        if (isBoss)
        {
            maxHP = Convert.ToInt32(startHP * waveHandler.CurrentWave * 3) - (2 * waveHandler.CurrentWave);
            healthBar.SetMaxHealth(maxHP);
        }
        else
        {
            maxHP = Convert.ToInt32(startHP * waveHandler.CurrentWave * 1.75) - (2 * waveHandler.CurrentWave);
            healthBar.SetMaxHealth(maxHP);
        }
    }

    public void SetEnemyHealth()
    {
        enemy = GetComponent<Enemy>();
        healthBar = GetComponent<HealthBar>();
        waveHandler = FindObjectOfType<WaveHandler>();
        healthBar.SetMaxHealth(startHP);
    }
}
