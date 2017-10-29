using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Spawner Variables")]
    [Tooltip("This gets changed depending on what level the game is on.")]
    public Wave currentWave;
    int currentWaveNumber;
    int enemiesAlive;
    int enemiesToSpawn;
    float nextSpawnTime;
    bool disabled;

    [Header("Key System Variables")]
    public int amountOfKeysInLevel;

    void Start()
    {
        //NextWave();
    }

    void Update()
    {
        if (!disabled)
        {
            if (enemiesToSpawn > 0 && Time.time > nextSpawnTime)
            {
                enemiesToSpawn--;
                nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;
                SpawnEnemy();
            }
            
            if(GameManager.instance.currentKeys >= amountOfKeysInLevel && enemiesToSpawn == 0)
            {
                //level over
                disabled = true;
            }

        }
    }

    void SpawnEnemy()
    {
        if(GameManager.instance.currentGameState == GameManager.GameState.PLAY)
        {
            int spawnPointIndex = Random.Range(0, GameManager.instance.spawnPoints.Count);
            Debug.Log(spawnPointIndex);
            Vector3 spawnPoint = GameManager.instance.spawnPoints[spawnPointIndex].gameObject.transform.position;
            Enemy spawnedEnemy = Instantiate(PickEnemyToSpawn(), spawnPoint, Quaternion.identity) as Enemy;
            spawnedEnemy.OnDeath += OnEnemyDeath;
        }
    }

    Enemy PickEnemyToSpawn()
    {
        return currentWave.typesOfEnemy[Random.Range(0, 3)];
    }

    void OnEnemyDeath()
    {
        enemiesAlive--;
        if (enemiesAlive == 0)
        {
            NextWave();
        }
    }

    public void NextWave()
    {
        currentWaveNumber++;
        print("Wave: " + currentWaveNumber);
        enemiesToSpawn = currentWave.amountOfEnemiesToSpawn;
        enemiesAlive = enemiesToSpawn;
    }

    [System.Serializable]
    public class Wave
    {
        public int timeBetweenWaves;
        public int amountOfEnemiesToSpawn;
        public float timeBetweenSpawns;
        public Enemy[] typesOfEnemy;

    }
}
