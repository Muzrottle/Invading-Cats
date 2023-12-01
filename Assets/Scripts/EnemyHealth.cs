using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHP = 5;

    [Tooltip("Adds amount of hitpoints when enemy dies.")]
    [SerializeField] int difficultyRamp = 1;
    
    int currentHP = 0;

    Enemy enemy;

    void OnEnable()
    {
        currentHP = maxHP;
    }

    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    private void OnParticleCollision(GameObject other)
    {
        ProcessHit();
    }

    private void ProcessHit()
    {
        currentHP--;

        Debug.Log("I'm Hit!");

        if (currentHP == 0)
        {
            Debug.Log("I died!");

            enemy.RewardGold();
            gameObject.SetActive(false);
            maxHP += difficultyRamp;
        }
    }
}
