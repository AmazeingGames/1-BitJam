using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBox : ColoredObject
{
    [SerializeField] new BoxCollider2D collider;
    [SerializeField] float defaultMass;

    public ColorSwap.Color CurrentState { get; private set; }
    
    Rigidbody2D rigidbody2D;

    private void Awake()
    {
        SetSpriteData();
    }

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        base.OnStart();
    }

    protected override void HandleColorSwap(ColorSwap.Color newColor)
    {
        CurrentState = newColor;

        SetSpriteData();

        spriteRenderer.sprite = SpriteData.ActiveSprite;

        playPhaseAnimation = true;

        SetPushability(newColor);
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

    void SetPushability(ColorSwap.Color newColor)
    {
        float newMass = newColor switch
        {
            ColorSwap.Color.White => defaultMass,
            ColorSwap.Color.Black => 1000,

            _ => throw new NotImplementedException(),
        };

        rigidbody2D.mass = newMass;
    }

    public override bool IsActiveCheck(ColorSwap.Color backgroundColor) => true;
}
