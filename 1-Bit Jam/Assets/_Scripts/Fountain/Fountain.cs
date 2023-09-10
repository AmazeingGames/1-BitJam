using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FMODUnity;

public class Fountain : ColoredObject
{
    [SerializeField] new BoxCollider2D collider;

    [Header("Sounds")]
    [SerializeField] bool overrideAttenuation;
    [SerializeField] float min;
    [SerializeField] float max;

    [SerializeField] GameObject heavenEmmitterChild;
    [SerializeField] GameObject hellEmmitterChild;


    StudioEventEmitter heavenEmmitter;
    StudioEventEmitter hellEmmitter;

    public ColorSwap.Color CurrentState { get; private set; }

    private void Awake()
    {
        SetSpriteData();
    }

    void Start()
    {
        heavenEmmitter = AudioManager.Instance.InitializeEventEmitter(AudioManager.EventSounds.HeavenlyFountain, heavenEmmitterChild);
        hellEmmitter = AudioManager.Instance.InitializeEventEmitter(AudioManager.EventSounds.HellishFountain, hellEmmitterChild);

        heavenEmmitter.Play();
        hellEmmitter.Play();

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

        switch (newColor)
        {
            case ColorSwap.Color.White:
                heavenEmmitter.Play();
                //hellEmmitter.Stop();
                break;

            case ColorSwap.Color.Black:
                //heavenEmmitter.Stop();
                hellEmmitter.Play();
                break;

            default:
                throw new NotImplementedException();
        }

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
