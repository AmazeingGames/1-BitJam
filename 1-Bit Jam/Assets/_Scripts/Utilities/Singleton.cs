using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A static instance is similar to a singleton, but instead of destryoing any new
/// instances, it overrides the current instance. This is useful for resetting the state
/// and saves you doing it manually
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        Instance = this as T;
    }

    protected virtual void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }
}

/// <summary>
/// This transforms the static instance into a basic singleton. This will destroy any new
/// versions created, leaving the original instance intact.
/// </summary>
/// <typeparam name="T"></typeparam>
/// 
public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        base.Awake();
    }
}

/*

/// <summary>
/// Finally we have a persistent version of the singleton. This will survive through scene
/// loads. Perfect for system calsses which require stateful, persisten data. Or aduio sources
/// where music plays through loading screens, etc..
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}
*/