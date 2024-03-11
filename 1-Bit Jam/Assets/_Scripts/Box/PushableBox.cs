using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBox : ColoredObject
{
    [SerializeField] new Rigidbody2D rigidbody;
    [SerializeField] new BoxCollider2D collider;
    [SerializeField] float defaultMass;

    public ColorSwap.Color CurrentState { get; private set; }

    private void Awake()
        => SetSpriteData();

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
        rigidbody.mass = newMass;
    }

    public override bool IsActiveCheck(ColorSwap.Color backgroundColor) => true;
}
