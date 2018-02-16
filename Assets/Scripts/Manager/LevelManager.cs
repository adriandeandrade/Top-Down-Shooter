using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Spawner Variables")]
    [Tooltip("This gets changed depending on what level the game is on.")]
    public Level currentLevel;
    int currentWaveNumber;
    int enemiesAlive;
    int enemiesToSpawn;
    float nextSpawnTime;
    [HideInInspector]
    public bool disabled;

    void Start()
    {
        //NextWave();
        disabled = true;
    }

    void Update()
    {
        if (disabled)
        {
            return;
        }



        if (enemiesToSpawn > 0 && Time.time > nextSpawnTime)
        {
            enemiesToSpawn--;
            nextSpawnTime = Time.time + currentLevel.timeBetweenSpawns;
            StartCoroutine(SpawnEnemy());
        }

        if (GameManager.instance.currentKeys >= currentLevel.amountOfKeys && enemiesToSpawn == 0)
        {
            //level over
            disabled = true;
        }
    }

    IEnumerator SpawnEnemy()
    {
        int spawnPointIndex = Random.Range(0, currentLevel.spawnPoints.Count);
        Vector3 spawnPoint = currentLevel.spawnPoints[spawnPointIndex].transform.position;
        Enemy enemyInstance = Instantiate(PickEnemyToSpawn(), spawnPoint, Quaternion.identity);
        enemyInstance.FindTarget();
        enemyInstance.OnDeath += OnEnemyDeath;
        yield return null;
    }


    //void SpawnEnemy()
    //{
        

    //    //if(GameManager.instance.currentGameState == GameManager.GameState.PLAY)
    //    //{
    //    //    int spawnPointIndex = Random.Range(0, GameManager.instance.spawnPoints.Count);
    //    //    Vector3 spawnPoint = GameManager.instance.spawnPoints[spawnPointIndex].gameObject.transform.position;
    //    //    Enemy spawnedEnemy = Instantiate(PickEnemyToSpawn(), spawnPoint, Quaternion.identity) as Enemy;
    //    //    spawnedEnemy.OnDeath += OnEnemyDeath;
    //    //}
    //}

    Enemy PickEnemyToSpawn()
    {
        return currentLevel.typesOfEnemy[Random.Range(0, currentLevel.typesOfEnemy.Length)];
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
        enemiesToSpawn = currentLevel.amountOfEnemiesToSpawn;
        enemiesAlive = enemiesToSpawn;
    }

    [System.Serializable]
    public class Level
    {
        public int timeBetweenWaves;
        public int amountOfEnemiesToSpawn;
        public int amountOfKeys;
        public float timeBetweenSpawns;
        public Transform playerSpawn;
        public Enemy[] typesOfEnemy;
        public List<Transform> spawnPoints = new List<Transform>();

    }
}
