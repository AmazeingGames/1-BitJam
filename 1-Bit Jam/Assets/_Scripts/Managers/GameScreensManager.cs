using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScreensManager : MonoBehaviour
{
    [SerializeField] Canvas loadingMenu;
    [SerializeField] Camera menuCamera;

    private void OnEnable()
    {
        GameManager.OnLoadStart += HandleLoadStart;
    }

    private void OnDisable()
    {
        GameManager.OnLoadStart -= HandleLoadStart;
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

        while (true)
        {
            Debug.Log("loading");
            if (loadingTask.isDone)
            {
                loadingMenu.gameObject.SetActive(false);
                menuCamera.gameObject.SetActive(false);
                Debug.Log("Finished loading");
                yield break;
            }
            
            yield return null;
        }
    }
}
