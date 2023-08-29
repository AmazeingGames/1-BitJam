using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] int testLevel;
    [SerializeField] List<LevelData> levelData = new();

    public LevelData LevelDataCurrent { get; private set; }

    public static event Action<GameState> OnStateLeave;
    public static event Action<GameState> OnStateEnter;

    public static event Action<AsyncOperation> OnLoadStart;

    public GameState PreviousState { get; private set; } = GameState.None;
    public GameState State { get; private set; } = GameState.None;
    public bool IsLevelPlaying { get; private set; }

    public enum GameState { None, MainMenu, Loading, LevelStart, GamePause, CreditsMenu, LevelFinish, Lose, GameFinish }

    string sceneToUnload = null;

    IEnumerator Start()
    {
        yield return null;

    #if DEBUG
        if (testLevel != 0 && DoesLevelExist(testLevel))
            UpdateGameState(GameState.LevelStart, testLevel);
        else if (testLevel != -1)
            UpdateGameState(GameState.MainMenu);
    #else
        UpdateGameState(GameState.MainMenu);
    #endif
    }

    void OnMainMenuEnter()
    {
        //Debug.Log("MainMenu Enter");

        LoadScene("_MainMenu");
    }

    void OnMainMenuExit()
    {
        //Debug.Log("MainMenu Exit");
    }

    void OnLoadingEnter(GameState loadingState)
    {
    }

    void OnLoadingFinish(GameState nextState)
    {
    }

    void OnLevelLoad(int levelToLoad)
    {
        LevelDataCurrent = levelData[levelToLoad - 1];

        LoadLevel(levelToLoad);

        IsLevelPlaying = true;
        Time.timeScale = 1.0f;
    }

    void OnLevelStop()
    {
        //Debug.Log("Level paused or finished");

        IsLevelPlaying = false;

        Time.timeScale = 0;
    }

    void OnGamePause()
    {
        //Debug.Log("Game Puased");
    }

    void OnGameResume()
    {
        //Debug.Log("Game Resumed");
    }

    void OnLevelFinish()
    {
        //Debug.Log("Level Finish");

        if (DoesLevelExist(LevelDataCurrent.Level + 1))
        {
            UpdateGameState(GameState.LevelStart, LevelDataCurrent.Level + 1); 
        }
        else
        {
            UpdateGameState(GameState.GameFinish);
        }
    }

    void OnLevelLose()
    {
        ReloadLevel();
    }

    void OnGameFinish()
    {
        LoadScene("_MainMenu");
    }

    void ReloadLevel()
    {
        UpdateGameState(GameState.LevelStart, LevelDataCurrent.Level);
    }

    bool LoadLevel(int level) => LoadScene($"Level_{level}");

    bool LoadScene(string sceneName)
    {
        //loadingCanvas.gameObject.SetActive(true);

        if (!DoesSceneExist(sceneName))
        {
            //loadingCanvas.gameObject.SetActive(false);
            return false;
        }

        UnloadScene(sceneToUnload);
        
        sceneToUnload = sceneName;

        UpdateGameState(GameState.Loading);

        Debug.Log($"Loading Scene: {sceneName}");

        var load = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        OnLoadStart?.Invoke(load);

        return true;
    }

    bool UnloadScene(string sceneName)
    {
        if (!DoesSceneExist(sceneName))
            return false;

        if (sceneToUnload == null)
            return false;

        Debug.Log($"Unloaded scene : {sceneName}");

        SceneManager.UnloadSceneAsync(sceneName);

        return true;
    }

    public static bool DoesLevelExist(int level) => DoesSceneExist($"Level_{level}");

    public static bool DoesSceneExist(string sceneName)
    {
        int buildIndex = SceneUtility.GetBuildIndexByScenePath(sceneName);

        if (buildIndex == -1)
        {
            return false;
        }
        return true;
    }

    public void UpdateGameState(GameState newState, int levelToLoad = -1)
    {
        OnStateLeave?.Invoke(State);

        PreviousState = State;
        State = newState;

        //Not sure if this is an issue, but this is always called the first time we exit a state, even when we haven't technically 'left' any states. This also happens in the main menu manager.

        Debug.Log($"Change State | New State: {newState} | Previous State: {PreviousState}");
        //State leave
        switch (PreviousState)
        {
            case GameState.None: 
                break;

            case GameState.MainMenu:
                OnMainMenuExit();
                break;

            case GameState.Loading:
                OnLoadingFinish(PreviousState);
                break;

            case GameState.LevelStart:
                OnLevelStop();
                break;

            case GameState.GamePause:
                OnGameResume();
                break;

            case GameState.LevelFinish:
                break;

            case GameState.Lose:
                break;

            case GameState.GameFinish:
                break;

            default:
                throw new NotImplementedException();
        }

        //State Enter
        switch (newState)
        {
            case GameState.None:
                break;

            case GameState.MainMenu:
                OnMainMenuEnter();
                break;

            case GameState.Loading:
                OnLoadingEnter(PreviousState);
                break;

            case GameState.LevelStart:
                Debug.Log("level Start");

                if (levelToLoad == -1)
                    throw new NotImplementedException();

                OnLevelLoad(levelToLoad);
                break;

            case GameState.GamePause:
                OnGamePause();
                break;

            case GameState.Lose:
                OnLevelLose();
                break;

            case GameState.LevelFinish:
                OnLevelFinish();
                break;

            case GameState.GameFinish:
                OnGameFinish();
                break;

            default:
                throw new NotImplementedException();
        }

        OnStateEnter?.Invoke(newState);
    }
}
