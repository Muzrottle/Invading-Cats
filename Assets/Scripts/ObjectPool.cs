using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject bossPrefab;
    [SerializeField][Range(0, 50)] int enemyPoolSize = 5;
    [SerializeField] [Range(0, 50)]int bossPoolSize = 3;
    [SerializeField] [Range(0.1f, 30f)]float spawnTime = 1f;
    [SerializeField] int bossCounter = 10;

    WaveHandler waveHandler;

    GameObject[] enemyPool;
    GameObject[] bossPool;
    Coroutine enemySpawner;
    int spawnedEnemyCounter = 0;

    void Start()
    {
        PopulatePool();
        waveHandler = FindObjectOfType<WaveHandler>();
    }

    private void PopulatePool()
    {
        enemyPool = new GameObject[enemyPoolSize];
        bossPool = new GameObject[bossPoolSize];

        for(int i = 0; i < enemyPool.Length; i++)
        {
            enemyPool[i] = Instantiate(enemyPrefab, transform);
            enemyPool[i].GetComponent<EnemyHealth>().SetEnemyHealth();
            enemyPool[i].SetActive(false);
        }

        for (int i = 0; i < bossPool.Length; i++)
        {
            bossPool[i] = Instantiate(bossPrefab, transform);
            bossPool[i].GetComponent<EnemyHealth>().SetEnemyHealth();
            bossPool[i].SetActive(false);
        }
    }

    private IEnumerator EnemySpawner()
    {
        while (true)
        {
            EnableEnemyObjectInPool();
            yield return new WaitForSeconds(spawnTime);
        }
    }

    private void EnableEnemyObjectInPool()
    {
        for(int i = 0; i < enemyPool.Length; i++)
        {
            if (spawnedEnemyCounter != 0 && spawnedEnemyCounter % bossCounter == 0)
            {
                spawnedEnemyCounter++;
                EnableBossObjectInPool();
                return;
            }

            if (!enemyPool[i].activeInHierarchy)
            {
                spawnedEnemyCounter++;
                enemyPool[i].SetActive(true);
                PoolController();
                return;
            }
        }
    }

    private void EnableBossObjectInPool()
    {
        for (int i = 0; i < bossPool.Length; i++)
        {
            if (!bossPool[i].activeInHierarchy)
            {
                bossPool[i].SetActive(true);
                PoolController();
                return;
            }
        }
    }

    public void ChangeAllPoolHealth()
    {
        for (int i = 0; i < enemyPool.Length; i++)
        {
            enemyPool[i].GetComponent<EnemyHealth>().SetNewHealth(false);
        }

        for (int i = 0;i < bossPool.Length; i++)
        {
            bossPool[i].GetComponent<EnemyHealth>().SetNewHealth(true);
        }
    }

    void PoolController()
    {
        bool isWaveLimitReached = waveHandler.CheckWaveSpawnEnemies(spawnedEnemyCounter);

        if (isWaveLimitReached)
        {
            DisablePool();
            spawnedEnemyCounter = 0;
        }
    }

    public void EnablePool()
    {
        enemySpawner = StartCoroutine(EnemySpawner());
    }

    public void DisablePool()
    {
        StopCoroutine(enemySpawner);
    }
}
