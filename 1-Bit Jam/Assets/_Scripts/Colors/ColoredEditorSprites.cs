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

    //Awake is only called when the script is instanced.
    void Awake()
    {
        runInEditMode = true;

        spriteRenderer = GetComponent<SpriteRenderer>();

        enemy = GetComponent<Enemy>();
        button = GetComponent<Button>();
    }

    //Update is only called when something in the Scene changed.
    void Update()
    {
        if (!Application.isPlaying)
        {
            SetSprite();
        }
    }

    //This code, obviously, is bad and would be drastically improved if both Enemy and Button inherited logic from the same script
    //However, we are on a time crunch here
    void SetSprite()
    {
        Debug.Log("Set Sprite");

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

        if (newSprite != null)
        {
            spriteRenderer.sprite = newSprite;
        }

    }
}
