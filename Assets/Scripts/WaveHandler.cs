using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveHandler : MonoBehaviour
{
    [SerializeField] float waveStartTimer = 5f;
    [SerializeField] GameObject showWave;

    ObjectPool pool;
    TextMeshProUGUI waveText;

    int currentWave = 0;
    public int CurrentWave { get { return currentWave; } }
    double waveEnemySize;
    int aliveEnemyCount = 0;
    bool isAllEnemiesSpawned = true;

    private void Start()
    {
        pool = FindObjectOfType<ObjectPool>();
        waveText = showWave.GetComponent<TextMeshProUGUI>();

        CheckWaveEnemies();
    }

    public bool CheckWaveSpawnEnemies(int currentEnemySpawned)
    {
        Debug.Log("WaveCounter = " + currentEnemySpawned);

        aliveEnemyCount++;

        if (currentEnemySpawned == waveEnemySize)
        {
            isAllEnemiesSpawned = true;
            return true;
        }

        return false;
    }

    public void LowerAliveCount()
    {
        aliveEnemyCount--;
        CheckWaveEnemies();
    }

    public void CheckWaveEnemies()
    {
        if (isAllEnemiesSpawned && aliveEnemyCount == 0)
        {
            Debug.Log("Girdim Be");
            showWave.SetActive(true);

            SetWave();
            pool.ChangeAllPoolHealth();

            waveText.DOFade(1f, waveStartTimer / 2)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                        waveText.DOFade(0f, waveStartTimer / 2)
                        .SetEase(Ease.Linear)
                        .OnComplete(() =>
                            StartNextWave()
                         )
                    );
        }
    }

    void SetWave()
    {
        currentWave++;
        Debug.Log("CurrentWave = " + currentWave);

        isAllEnemiesSpawned = false;
        waveText.text = "Wave " + currentWave.ToString();
        waveEnemySize = Mathf.Pow(currentWave + 1, 2);
        waveEnemySize = waveEnemySize - (4 * (currentWave - 1));

        Debug.Log("WaveSize = " + waveEnemySize);

    }

    void StartNextWave()
    {
        showWave.SetActive(false);
        pool.EnablePool();
    }
}
