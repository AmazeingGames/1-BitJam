using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class ColoredEditorSprites : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Enemy enemy;
    Button button;
    Exit exit;

    //Awake is only called when the script is instanced.
    void Awake()
    {
        runInEditMode = true;

        spriteRenderer = GetComponent<SpriteRenderer>();

        enemy = GetComponent<Enemy>();
        button = GetComponent<Button>();
        exit = GetComponent<Exit>();

    }

    //Update is only called when something in the Scene changed.
    void Update()
    {
        if (!Application.isPlaying)
        {
            SetSprite();
        }
    }

    //This code, obviously, is pretty bad and would be drastically improved if both Enemy and Button inherited logic from the same script or if I could just grab SpriteData directly
    //However, we are on a time crunch here
    void SetSprite()
    {

        Sprite newSprite = null;

        if (enemy != null)
        {
            newSprite = enemy.Color switch
            {
                ColorSwap.Color.White => enemy.LightEnemyData.SpriteData.ActiveSprite,
                ColorSwap.Color.Black => enemy.DarkEnemyData.SpriteData.ActiveSprite,
                ColorSwap.Color.Neutral => throw new NotImplementedException(),
                _ => throw new Exception(),
            };
        }

        else if (button != null)
        {
            newSprite = button.Color switch
            {
                ColorSwap.Color.White => button.LightButtonData.SpriteData.ActiveSprite,
                ColorSwap.Color.Black => button.DarkButtonData.SpriteData.ActiveSprite,
                ColorSwap.Color.Neutral => throw new NotImplementedException(),
                _ => throw new Exception(),
            };
        }

        else if (exit != null)
        {
            newSprite = exit.Color switch
            {
                ColorSwap.Color.White => exit.LightExitData.ActiveSprite,
                ColorSwap.Color.Black => exit.DarkExitData.ActiveSprite,
                ColorSwap.Color.Neutral => throw new NotImplementedException(),
                _ => throw new Exception(),
            };
        }

        if (newSprite != null)
        {
            spriteRenderer.sprite = newSprite;
        }

    }
}
