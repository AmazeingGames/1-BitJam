using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This, Button, and Enemy clearly share some repeating qualities.
//TO DO: Make changes to reduce repeated code.
public class Exit : ColoredObject
{
    [SerializeField] bool showDebug;



    // Start is called before the first frame update
    void Awake()
    {
        SetSpriteData();
    }

    void Start()
    {
        OnStart();
    }


    void Update()
    {
        if (!GameManager.Instance.IsLevelPlaying)
            return;

        CheckAnimations();
    }

    

    protected override void HandleColorSwap(ColorSwap.Color newColor)
    {
        IsActiveProperty = IsActiveCheck(newColor);

        base.HandleColorSwap(newColor);
    }

    public override bool IsActiveCheck(ColorSwap.Color backgroundColor)
    {
        if (Color == ColorSwap.Color.Neutral)
            return true;

        return Color != backgroundColor;
    }
}
