using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    public static EnemySpawner instance;
    [SerializeField] private KeypointManager keypointManager;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject victoryText;
    [SerializeField] private GameObject threeStars;
    [SerializeField] private GameObject twoStars;
    [SerializeField] private GameObject oneStar;
    [SerializeField] private GameObject startButton;
    public bool levelStarted = false;
    public AudioSource bgm;
    public AudioClip bossMusic;
    public AudioClip winMusic3stars;
    public AudioClip winMusic2stars;
    public AudioClip winMusic1star;

    [Header("Wave Settings")]
    [SerializeField] private List<Wave> waves;

    [Header("Events")]
    public static UnityEvent OnEnemyDestroy = new UnityEvent();

    private int currentWaveIndex = -1;
    private int enemiesAlive = 0;
    private bool isSpawning = false;

    private void Awake()
    {
        instance = this;
        keypointManager = FindAnyObjectByType<KeypointManager>();
        OnEnemyDestroy.AddListener(EnemyDestroyed);
    }
    public void OnStartClick()
    {
        bgm.Play();
        StartNextWave();
        levelStarted = true;
        startButton.SetActive(false);
    }

    private void Start()
    {
        victoryText.SetActive(false);
        int totalEnemies = CalculateTotalEnemies();
        LevelStatsManager.Instance.SetTotalEnemies(totalEnemies);
        levelStarted = false;
        startButton.SetActive(true);
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;

        if (enemiesAlive == 0
            && currentWaveIndex >= waves.Count - 1
            && !isSpawning
            && LevelStatsManager.Instance.enemyNumber >= LevelStatsManager.Instance.TotalEnemies)
        {
            victoryText.SetActive(true);
            int stars = LevelStatsManager.Instance.GetStars();
            if (stars == 3)
            {
                bgm.clip = winMusic3stars;
                bgm.Play();
                threeStars.SetActive(true);
            }
            else if (stars == 2)
            {
                bgm.clip = winMusic2stars;
                bgm.Play();
                twoStars.SetActive(true);
            }
            else
            {
                bgm.clip = winMusic1star;
                bgm.Play();
                oneStar.SetActive(true);
            }
        }

        if (enemiesAlive == 0 && !isSpawning)
        {
            StartCoroutine(WaveDelay());
        }
    }


    private IEnumerator WaveDelay()
    {
        Wave wave = waves[currentWaveIndex];
        float waitTime = wave.timeBeforeNextWave;
        yield return new WaitForSeconds(waitTime);

        StartNextWave();
    }
    private void StartNextWave()
    {
        currentWaveIndex++;
        if (currentWaveIndex >= waves.Count)
        {
            return;
        }
        if (waves[currentWaveIndex].isBossWave == true)
        {
            bgm.clip = bossMusic;
            bgm.Play();
        }

        StartCoroutine(RunWave(waves[currentWaveIndex]));
    }

    private IEnumerator RunWave(Wave wave)
    {
        isSpawning = true;

        foreach (EnemyGroup group in wave.enemyGroups)
        {
            for (int i = 0; i < group.amount; i++)
            {
                SpawnEnemy(group.enemyType);
                enemiesAlive++;

                yield return new WaitForSeconds(1f / wave.enemiesPerSecond);
            }
        }

        isSpawning = false;
    }

    private void SpawnEnemy(int typeIndex)
    {
        if (typeIndex < 0 || typeIndex >= enemyPrefabs.Length)
        {
            Debug.LogError("Enemy type index out of range: " + typeIndex);
            return;
        }

        Instantiate(
            enemyPrefabs[typeIndex],
            keypointManager.GetNextWaypoint(0).position,
            Quaternion.identity
        );

        LevelStatsManager.Instance.IncreaseEnemyCount();
    }

    private int CalculateTotalEnemies()
    {
        int total = 0;

        foreach (Wave wave in waves)
        {
            foreach (EnemyGroup group in wave.enemyGroups)
            {
                total += group.amount;
            }
        }

        return total;
    }
}

[System.Serializable]
public class EnemyGroup
{
    public int enemyType;
    public int amount;
}

[System.Serializable]
public class Wave
{
    public bool isBossWave;
    public float enemiesPerSecond = 1f;
    public float timeBeforeNextWave = 5f;
    public List<EnemyGroup> enemyGroups = new List<EnemyGroup>();
}
