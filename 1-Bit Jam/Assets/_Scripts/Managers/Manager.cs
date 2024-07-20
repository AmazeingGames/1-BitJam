using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager
{
    public static bool hasLoadedScene = false;

    public static void InstanceNullCheck<T>(T instance) where T : Singleton<T>
    {
        if (instance != null || hasLoadedScene)
            return;

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.name == "_Game")
                return;
        }

        SceneManager.LoadScene("_Game", LoadSceneMode.Additive);
        hasLoadedScene = true;
    }
}

