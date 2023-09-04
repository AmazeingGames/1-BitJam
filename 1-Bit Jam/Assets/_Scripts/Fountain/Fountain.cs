using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fountain : ColoredObject
{
    [SerializeField] new BoxCollider2D collider;
    public ColorSwap.Color CurrentState { get; private set; }

    protected override void HandleColorSwap(ColorSwap.Color newColor)
    {
        base.HandleColorSwap(newColor);

        CurrentState = newColor;
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
                break;

            case ColorSwap.Color.Black:
                Debug.Log("You dead.");
                break;

            default:
                throw new NotImplementedException();
        }
    }
}
