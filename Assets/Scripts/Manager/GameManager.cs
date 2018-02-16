using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public enum GameState { NONE, MENU, INIT, HELP, PLAY, GAMEOVER, PAUSE, RESUME, INTRO }
    public GameState currentGameState = GameState.NONE;

    public GameObject uiMenu;
    public GameObject uiPlay;
    public GameObject uiPause;
    public GameObject uiHelp;

    public GameObject playerPrefab;
    public GameObject playerPrefabInstance;

    public int currentKeys;
    public int currentLevel;

    public SceneFader sceneFader;

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
                //LoadLevel("Menu");
                uiMenu.SetActive(true);
                break;
            case GameState.INIT:
                LoadLevel("Level01");
                //ClearGame();
                //playerPrefabInstance = GameObject.Instantiate(playerPrefab);
                //currentKeys = 0;
                //currentLevel = 1;
                //LoadLevel(1);
                //playerPrefabInstance.transform.position = levelManager.currentLevel.playerSpawn.position;
                SwitchState(GameState.PLAY);
                break;
            case GameState.INTRO:
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

            case GameState.INTRO:
                break;
            case GameState.PLAY:
                ClearGame();
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
        switch (currentGameState)
        {
            case GameState.MENU:
                break;
            case GameState.INIT:
                break;
            case GameState.INTRO:
                break;
            case GameState.PLAY:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    SwitchState(GameState.PAUSE);
                }
                
                break;
            case GameState.PAUSE:
                break;
            case GameState.RESUME:
                break;
        }
    }

    public void ClearGame()
    {
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

    void LoadLevel(string levelName)
    {
        currentKeys = 0;
        sceneFader.FadeTo(levelName);
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
        SwitchState(GameState.MENU);
    }
}
