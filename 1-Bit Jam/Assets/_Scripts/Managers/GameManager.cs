using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
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
        // Starts the game by unloading and reloading the level already in the scene
        int levelToLoad = -1;
        string levelString;

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);

            if (scene.name[..5] != "Level")
                continue;

            levelString = scene.name[(scene.name.LastIndexOf('_') + 1)..];

            if (int.TryParse(levelString, out int levelNumber))
            {
                previousScene = scene.name;
                levelToLoad = levelNumber;
            }
        }

        if (DoesLevelExist(levelToLoad))
        {
            UpdateGameState(GameState.LevelStart, levelToLoad);
        }
        else
            UpdateGameState(GameState.MainMenu);
        
    #else
        UpdateGameState(GameState.MainMenu);
    #endif
    }

    public static void StopGame()
    {
        Manager.InstanceNullCheck();

        IsGameRunning = false;
        GameStop?.Invoke();
    }

    public static void StartGame()
    {
        Manager.InstanceNullCheck();

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
        Manager.InstanceNullCheck();

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

    // Asynchronously loads the given scene, while unloading the last loaded scene
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
