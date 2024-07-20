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

    public static event Action<AsyncOperation, bool> OnLoadStart;

    public static event Action GameStart;
    public static event Action GameStop;

    public GameState PreviousState { get; private set; } = GameState.None;
    public GameState State { get; private set; } = GameState.None;
    public static bool IsGameRunning { get; private set; } 

    public enum GameState { None, MainMenu, Loading, LevelStart, LevelRestart, GamePause, CreditsMenu, LevelFinish, Lose, GameFinish }

    string previousScene = null;

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

    public static void StopGame()
    {
        Manager.InstanceNullCheck(Instance);

        IsGameRunning = false;
        GameStop?.Invoke();
    }

    public static void StartGame()
    {
        Manager.InstanceNullCheck(Instance);

        IsGameRunning = true;
        GameStart?.Invoke();
    }

    public static bool DoesLevelExist(int level) => DoesSceneExist($"Level_{level}");

    public static bool DoesSceneExist(string sceneName)
    {
        int buildIndex = SceneUtility.GetBuildIndexByScenePath(sceneName);

        return buildIndex != -1;
    }

    public static void UpdateGameState(GameState newState, int levelToLoad = -1)
    {
        Manager.InstanceNullCheck(Instance);

        OnStateLeave?.Invoke(Instance.State);

        Instance.PreviousState = Instance.State;
        Instance.State = newState;

        //Not sure if this is an issue, but this is always called the first time we exit a state, even when we haven't technically 'left' any states. This also happens in the main menu manager.

        switch (newState)
        {
            case GameState.MainMenu:
                Instance.LoadScene("_MainMenu");
            break;

            case GameState.LevelStart:
                if (levelToLoad == -1)
                    throw new NotImplementedException();

                Instance.LevelDataCurrent = Instance.levelData[levelToLoad - 1];

                Instance.LoadLevel(levelToLoad);
            break;

            case GameState.Lose:
                StopGame();
            break;

            case GameState.LevelRestart:
                UpdateGameState(GameState.LevelStart, Instance.LevelDataCurrent.Level);
            break;

            case GameState.LevelFinish:
                if (DoesLevelExist(Instance.LevelDataCurrent.Level + 1))
                    UpdateGameState(GameState.LevelStart, Instance.LevelDataCurrent.Level + 1);
                else
                    UpdateGameState(GameState.GameFinish); 
            break;

            case GameState.GameFinish:
                Instance.LoadScene("_MainMenu");
            break;
        }

        OnStateEnter?.Invoke(newState);
    }

    bool LoadLevel(int level) => LoadScene($"Level_{level}", true);


    bool LoadScene(string sceneName, bool isLevel = false)
    {
        if (!DoesSceneExist(sceneName))
            return false;

        UnloadScene(previousScene);

        previousScene = sceneName;

        UpdateGameState(GameState.Loading);

        var load = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        OnLoadStart?.Invoke(load, isLevel);

        return true;
    }

    bool UnloadScene(string sceneName)
    {
        if (!DoesSceneExist(sceneName))
            return false;

        if (previousScene == null)
            return false;

        SceneManager.UnloadSceneAsync(sceneName);

        return true;
    }
}
