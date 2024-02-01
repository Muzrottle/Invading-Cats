using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] ParticleSystem projectileParticles;
    public float range = 15f;
    Transform target;

    // Update is called once per frame
    void Update()
    {
        FindClosestTarget();
        AimAtEnemy();
    }

    private void FindClosestTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
        {
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);

            if (targetDistance < maxDistance)
            {
                closestTarget = enemy.transform;
                maxDistance = targetDistance;
            }
        }

        target = closestTarget;
    }

    private void AimAtEnemy()
    {
        if (target == null)
        {
            Attack(false);
            return;
        }

        float targetDistance = Vector3.Distance(transform.position, target.transform.position);
        
        if (targetDistance < range)
        {
            transform.LookAt(target);
            Attack(true);
        }
        else
        {
            Attack(false);
        }
    }

    private void Attack(bool isActive)
    {
        var emissionModule = projectileParticles.emission;

        emissionModule.enabled = isActive;
    }
}
