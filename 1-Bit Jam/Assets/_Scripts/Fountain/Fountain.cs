using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fountain : ColoredObject
{
    [SerializeField] new BoxCollider2D collider;
    public ColorSwap.Color CurrentState { get; private set; }

    private void Awake()
    {
        SetSpriteData();
    }

    void Start()
    {
        base.OnStart();    
    }

    void Update()
    {
        CheckAnimations();    
    }

    protected override void HandleColorSwap(ColorSwap.Color newColor)
    {
        CurrentState = newColor;

        SetSpriteData();
        
        animator.runtimeAnimatorController = SpriteData.Controller;

        spriteRenderer.sprite = SpriteData.ActiveSprite;

        playPhaseAnimation = true;
    }

    protected override void SetSpriteData()
    {
        SpriteData = CurrentState switch
        {
            ColorSwap.Color.White => LightSpriteData,
            ColorSwap.Color.Black => DarkSpriteData,

            _ => throw new NotImplementedException(),
        };
    }

    public override bool IsActiveCheck(ColorSwap.Color backgroundColor)
    {
        return true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (CurrentState)
        {
            case ColorSwap.Color.White:
                Debug.Log("You Good.");
                //Play splash
                break;

            case ColorSwap.Color.Black:
                GameManager.Instance.UpdateGameState(GameManager.GameState.Lose);
                break;

            default:
                throw new NotImplementedException();
        }
    }

    //private void OnTriggerStay2D(Collider2D collision) { }
}
