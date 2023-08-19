using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : PersistentSingleton<GameManager>
{
    [SerializeField] Canvas mainMenu;
    [SerializeField] Canvas levelSelect;

    public static event Action<GameState> OnStateLeave;
    public static event Action<GameState> OnStateChanged;

    public GameState PreviousState { get; private set; }
    public GameState State { get; private set; }

    public enum GameState { MainMenu, LevelSelect, LevelStart, CreditsMenu, Win, Lose }

    void Start()
    {
        UpdateGameState(GameState.MainMenu);
    }

    void OnMainMenuEnter()
    {
        Debug.Log("MainMenu Enter");
        mainMenu.gameObject.SetActive(true);
        levelSelect.gameObject.SetActive(false);
    }

    void OnMainMenuExit()
    {
        Debug.Log("MainMenu Exit");

        mainMenu.gameObject.SetActive(false);
    }

    void OnLevelSelectEnter()
    {
        Debug.Log("Level Select Enter");

        levelSelect.gameObject.SetActive(true);
    }

    void OnLevelSelectExit()
    {
        Debug.Log("Level Select Exit");

        levelSelect.gameObject.SetActive(false);
    }

    void OnLevelStart()
    {

    }

    void OnLevelLose()
    {

    }

    void OnLevelWin()
    {

    }


    public void UpdateGameState(GameState newState)
    {
        OnStateLeave?.Invoke(newState);

        switch (State)
        {
            case GameState.MainMenu:
                OnMainMenuExit();
                break;
            case GameState.LevelSelect:
                OnLevelSelectExit();
                break;
            case GameState.LevelStart:
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                break;
            default:
                throw new NotImplementedException();
        }

        PreviousState = State;
        State = newState;

        switch (newState)
        {
            case GameState.MainMenu:
                OnMainMenuEnter();
                break;

            case GameState.LevelSelect:
                OnLevelSelectEnter();
                break;

            case GameState.LevelStart:
                OnLevelStart();
                break;

            case GameState.Win:
                OnLevelWin();
                break;

            case GameState.Lose:
                OnLevelLose();
                break;

            default:
                throw new NotImplementedException();
        }

        OnStateChanged?.Invoke(newState);
    }

    public void LoadLevel(int level)
    {
        Debug.Log($"changed scene to level {level}");

        SceneManager.LoadScene($"Level_{level}");
    }
}
