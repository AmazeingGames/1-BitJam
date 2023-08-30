using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScreensManager : Singleton<GameScreensManager>
{
    [field: SerializeField] public Canvas GameOver { get; private set; }
    [SerializeField] Canvas loadingMenu;
    [SerializeField] Canvas settingsMenu;
    [field: SerializeField] public Canvas GameSettingsBarCanvas { get; private set; }
    [SerializeField] GameObject gameSettingsBarLayout;
    [SerializeField] Canvas creditsMenu;

    [SerializeField] Camera menuCamera;

    void OnEnable()
    {
        GameManager.OnLoadStart += HandleLoadStart;
        GameManager.OnStateEnter += HandleLevelLose;

        GameManager.GameStart += HandleGameStart;
        GameManager.GameStop += HandleGameStop;

        RestartButton.OnRestart += HandleLevelRestart;
        ExitToMenuButton.OnExit += HandleExitToMenu;
        CreditsButton.OnCredits += HandleCredits;
        SettingsButton.OnSettings += HandleSettings;
    }

    void OnDisable()
    {
        GameManager.OnLoadStart -= HandleLoadStart;
        GameManager.OnStateEnter -= HandleLevelLose;

        GameManager.GameStart -= HandleGameStart;
        GameManager.GameStop -= HandleGameStop;

        RestartButton.OnRestart -= HandleLevelRestart;
        ExitToMenuButton.OnExit -= HandleExitToMenu;
        CreditsButton.OnCredits -= HandleCredits;
        SettingsButton.OnSettings -= HandleSettings;
    }

    void Start()
    {
        GameOver.enabled = false;
        GameOver.gameObject.SetActive(true);

        GameSettingsBarCanvas.enabled = true;
        GameSettingsBarCanvas.gameObject.SetActive(false);

        settingsMenu.enabled = false;
        settingsMenu.gameObject.SetActive(true);

        creditsMenu.enabled = false;
        creditsMenu.gameObject.SetActive(true);

        
    }

    void HandleGameStart()
    {
        Debug.Log("Handled game start");
        GameSettingsBarCanvas.gameObject.SetActive(true);
    }

    void HandleGameStop()
    {
        Debug.Log("Handled game stop");
        GameSettingsBarCanvas.gameObject.SetActive(false);
    }

    void HandleSettings()
    {
        Debug.Log("Handled Settings");

        gameSettingsBarLayout.SetActive(!gameSettingsBarLayout.activeSelf);
    }
    
    void HandleCredits()
    {
        Debug.Log("Handled Credits");

        creditsMenu.enabled = !creditsMenu.enabled;

        if (creditsMenu.enabled)
            GameManager.Instance.StopGame();
        else
            GameManager.Instance.StartGame();
    }

    void HandleLevelLose(GameManager.GameState gameState)
    {
        if (gameState == GameManager.GameState.Lose)
        {
            Debug.Log("Handled Level Lose");
            GameOver.enabled = true;
        }
    }

    void HandleLevelRestart()
    {
        Debug.Log("Handled Game Restart");

        GameManager.Instance.UpdateGameState(GameManager.GameState.LevelRestart);
        GameOver.enabled = false;
    }

    void HandleExitToMenu()
    {
        Debug.Log("Handle Menu Exit");

        GameManager.Instance.UpdateGameState(GameManager.GameState.MainMenu);
        GameOver.enabled = false;
    }

    void HandleLoadStart(AsyncOperation loadingTask, bool startGame)
    {
        Debug.Log("Enter loading menu");

        StartCoroutine(Loading(loadingTask, startGame));
    }

    IEnumerator Loading(AsyncOperation loadingTask, bool loadingLevel)
    {
        loadingMenu.gameObject.SetActive(true);
        menuCamera.gameObject.SetActive(true);

        GameManager.Instance.StopGame();

        while (true)
        {
            if (loadingTask.isDone)
            {
                yield return null;

                loadingMenu.gameObject.SetActive(false);
                menuCamera.gameObject.SetActive(false);
                Debug.Log("Finished loading");

                if (loadingLevel)
                {
                    GameManager.Instance.StartGame();
                }

                yield break;
            }
            
            yield return null;
        }
    }
}
