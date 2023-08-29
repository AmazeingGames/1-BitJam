using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScreensManager : MonoBehaviour
{
    [SerializeField] Canvas gameOver;
    [SerializeField] Canvas loadingMenu;
    [SerializeField] Camera menuCamera;

    void OnEnable()
    {
        GameManager.OnLoadStart += HandleLoadStart;
        GameManager.OnStateEnter += HandleLevelLose;

        RestartButton.OnRestart += HandleLevelRestart;
        ExitToMenuButton.OnExit += HandleExitToMenu;
    }

    void OnDisable()
    {
        GameManager.OnLoadStart -= HandleLoadStart;
        GameManager.OnStateEnter -= HandleLevelLose;

        RestartButton.OnRestart -= HandleLevelRestart;
        ExitToMenuButton.OnExit -= HandleExitToMenu;
    }

    void Start()
    {
        gameOver.enabled = false;
    }

    void HandleLevelLose(GameManager.GameState gameState)
    {
        if (gameState == GameManager.GameState.Lose)
        {
            Debug.Log("Handled Level Lose");
            gameOver.enabled = true;
        }
    }

    void HandleLevelRestart()
    {
        Debug.Log("Handled Game Restart");

        GameManager.Instance.UpdateGameState(GameManager.GameState.LevelRestart);
        gameOver.enabled = false;
    }

    void HandleExitToMenu()
    {
        Debug.Log("Handle Menu Exit");

        GameManager.Instance.UpdateGameState(GameManager.GameState.MainMenu);
        gameOver.enabled = false;
    }

    void HandleLoadStart(AsyncOperation loadingTask)
    {
        Debug.Log("Enter loading menu");

        StartCoroutine(Loading(loadingTask));
    }

    IEnumerator Loading(AsyncOperation loadingTask)
    {
        loadingMenu.gameObject.SetActive(true);
        menuCamera.gameObject.SetActive(true);

        GameManager.Instance.StopGame();

        while (true)
        {
            if (loadingTask.isDone)
            {
                loadingMenu.gameObject.SetActive(false);
                menuCamera.gameObject.SetActive(false);
                Debug.Log("Finished loading");

                GameManager.Instance.StartGame();

                yield break;
            }
            
            yield return null;
        }
    }
}
