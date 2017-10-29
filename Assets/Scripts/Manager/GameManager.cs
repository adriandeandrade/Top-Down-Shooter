using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public enum GameState { NONE, MENU, INIT, HELP, PLAY, GAMEOVER, PAUSE, RESUME }
    public GameState currentGameState = GameState.NONE;

    public GameObject uiMenu;
    public GameObject uiPlay;
    public GameObject uiPause;
    public GameObject uiHelp;
    public GameObject[] levelPrefabs;
    public List<GameObject> spawnPoints = new List<GameObject>();

    public GameObject playerPrefab;
    public GameObject playerPrefabInstance;

    private GameObject levelPrefab;

    public LevelManager levelManager;

    public int currentKeys;
    public int currentLevel;

    void Start()
    {
        SwitchState(GameState.MENU);
    }

    void SwitchState(GameState newState)
    {
        ExitState();
        InitState(newState);
    }

    void InitState(GameState newState)
    {
        currentGameState = newState;
        switch (currentGameState)
        {
            case GameState.MENU:
                uiMenu.SetActive(true);
                break;
            case GameState.INIT:
                ClearGame();
                playerPrefabInstance = GameObject.Instantiate(playerPrefab);
                playerPrefabInstance.transform.position = Vector3.zero + (Vector3.up * 2);
                currentKeys = 0;
                currentLevel = 1;
                LoadLevel(1);
                SwitchState(GameState.PLAY);
                break;
            case GameState.HELP:
                uiHelp.SetActive(false);
                break;
            case GameState.PLAY:
                uiPlay.SetActive(true);
                break;
            case GameState.PAUSE:
                uiPause.SetActive(true);
                break;
            case GameState.RESUME:
                break;
        }
    }

    void ExitState()
    {
        switch (currentGameState)
        {
            case GameState.MENU:
                uiMenu.SetActive(false);
                break;
            case GameState.INIT:
                break;
            case GameState.HELP:
                uiHelp.SetActive(false);
                break;
            case GameState.PLAY:
                uiPlay.SetActive(false);
                break;
            case GameState.PAUSE:
                uiPause.SetActive(false);
                break;
            case GameState.RESUME:
                break;

        }
    }

    void Update()
    {
        if (currentGameState != GameState.PAUSE) return;
        
        if (currentGameState == GameState.PAUSE)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SwitchState(GameState.PAUSE);
            }
        }
    }


    public void ClearGame()
    {
        if (levelPrefab != null)
        {
            Destroy(levelPrefab);
        }

        if (playerPrefabInstance != null)
        {
            Destroy(playerPrefabInstance);
        }

        Enemy[] enemiesInScene = FindObjectsOfType<Enemy>();
        for (int i = 0; i < enemiesInScene.Length; i++)
        {
            Destroy(enemiesInScene[i].gameObject);
        }
    }

    void LoadLevel(int levelNumber)
    {
        if (levelPrefab != null) Destroy(levelPrefab);
        levelPrefab = GameObject.Instantiate(levelPrefabs[levelNumber - 1]);
        currentKeys = 0;
        currentLevel = levelNumber;
        Invoke("FindAllSpawnPoints", 2f);
    }

    void FindAllSpawnPoints()
    {
        foreach (GameObject spawnPoint in GameObject.FindGameObjectsWithTag("Spawnpoint"))
        {
            spawnPoints.Add(spawnPoint);
        }
        StartEnemies();
    }

    void StartEnemies()
    {
        levelManager = GetComponent<LevelManager>();
        levelManager.NextWave();
    }

    public void CollectKey()
    {
        currentKeys++;
        // Play key pickup sound
    }

    public void PressPlay()
    {
        SwitchState(GameState.INIT);
    }

    public void PressHelp()
    {
        SwitchState(GameState.HELP);
    }

    public void PressQuit()
    {
        Application.Quit();
    }

    public void PressResume()
    {
        SwitchState(GameState.PLAY);
    }

    public void PressMenu()
    {
        ClearGame();
        SwitchState(GameState.MENU);
    }
}
