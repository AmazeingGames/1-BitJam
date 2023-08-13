using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;

    public GameState State { get; private set; }

    public enum GameState { Start, Win, Lose }
    
    void Start()
    {
        //ChangeState();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeState(GameState newState)
    {
        OnBeforeStateChanged?.Invoke(newState);

        State = newState;

        switch (newState)
        {
            case GameState.Start:
                break;
            
            case GameState.Win:
                break;
            
            case GameState.Lose:
                break;
        }

        OnAfterStateChanged?.Invoke(newState);
    }
}
