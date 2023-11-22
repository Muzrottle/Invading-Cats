using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHP = 5;
    int currentHP = 0;


    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
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

            Destroy(gameObject);
        }
    }
}
