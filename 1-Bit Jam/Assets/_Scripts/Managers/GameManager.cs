using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : PersistentSingleton<GameManager>
{
    [SerializeField] GameState levelStartingState;
    [SerializeField] LevelData levelData;

    [SerializeField] Canvas mainMenu;
    [SerializeField] Canvas levelSelect;
    [SerializeField] Canvas loadingCanvas;

    public static event Action<GameState> OnStateLeave;
    public static event Action<GameState> OnStateChanged;

    public GameState PreviousState { get; private set; }
    public GameState State { get; private set; }
    public bool IsLevelPlaying { get; private set; }

    public enum GameState { MainMenu, LevelSelectMenu, Loading, LevelStart, GamePause, CreditsMenu, LevelFinish, Lose }

    CanSwapColor canSwapColor;

    IEnumerator Start()
    {
        canSwapColor = gameObject.AddComponent<CanSwapColor>();

        yield return null;

        UpdateGameState(levelStartingState);
    }

    void OnMainMenuEnter()
    {
        Debug.Log("MainMenu Enter");

        mainMenu.gameObject.SetActive(true);
        levelSelect.gameObject.SetActive(false);
        loadingCanvas.gameObject.SetActive(false);
    }

    void OnMainMenuExit()
    {
        Debug.Log("MainMenu Exit");

        mainMenu.gameObject.SetActive(false);
    }

    void OnLevelSelectMenuEnter()
    {
        Debug.Log("Level Select Enter");

        levelSelect.gameObject.SetActive(true);
    }

    void OnLevelSelectMenuExit()
    {
        Debug.Log("Level Select Exit");

        levelSelect.gameObject.SetActive(false);
    }

    void OnLodaingEnter()
    {
        Debug.Log("Level Loading");
    }

    void OnLoadingExit()
    {
        Debug.Log("Loading finished");
    }

    void OnLevelStart()
    {
        Debug.Log("On level start");

        if (levelData == null)
            throw new Exception("Level data not set.");

        ColorSwap.Instance.ChangeColor(levelData.StartingColor, gameObject);

        IsLevelPlaying = true;
        Time.timeScale = 1.0f;
    }

    void OnLevelStop()
    {
        Debug.Log("Level paused or finished");

        IsLevelPlaying = false;

        Time.timeScale = 0;
    }

    void OnLevelFinish()
    {
        Debug.Log("Level Finish");
    }

    void OnLevelLose()
    {

    }


    public void UpdateGameState(GameState newState)
    {
        OnStateLeave?.Invoke(newState);

        PreviousState = State;

        switch (State)
        {
            case GameState.MainMenu:
                OnMainMenuExit();
                break;

            case GameState.LevelSelectMenu:
                OnLevelSelectMenuExit();
                break;

            case GameState.Loading:
                OnLoadingExit();
                break;

            case GameState.LevelStart:
                OnLevelStop();
                break;

            case GameState.GamePause:
                Debug.Log("Left game pause");
                break;

            case GameState.LevelFinish:
                Debug.Log("Left level finish state");
                break;


            case GameState.Lose:
                break;

            default:
                throw new NotImplementedException();
        }

        State = newState;

        switch (newState)
        {
            case GameState.MainMenu:
                OnMainMenuEnter();
                break;

            case GameState.LevelSelectMenu:
                OnLevelSelectMenuEnter();
                break;

            case GameState.Loading:
                OnLodaingEnter();
                break;

            case GameState.LevelStart:
                OnLevelStart();
                break;

            case GameState.GamePause:
                Debug.Log("game paused");
                break;

            case GameState.Lose:
                OnLevelLose();
                break;

            case GameState.LevelFinish:
                OnLevelFinish();
                break;

            default:
                throw new NotImplementedException();
        }

        OnStateChanged?.Invoke(newState);
    }

    public void LoadLevel(int level)
    {
        Debug.Log($"changed scene to level {level}");

        UpdateGameState(GameState.Loading);

        SceneManager.LoadScene($"Level_{level}");
    }
}
