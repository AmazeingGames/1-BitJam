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
        #if UNITY_EDITOR
        runInEditMode = true;

        spriteRenderer = GetComponent<SpriteRenderer>();

        enemy = GetComponent<Enemy>();
        button = GetComponent<Button>();
        exit = GetComponent<Exit>();
        #endif 

    }

    //Update is only called when something in the Scene changed.
    void Update()
    {
        #if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            SetSprite();
        }
        #endif
    }

    //This code, obviously, is pretty bad and would be drastically improved if both Enemy and Button inherited logic from the same script or if I could just grab SpriteData directly
    //However, we are on a time crunch here
    void SetSprite()
    {

        Sprite newSprite = null;

        if (enemy != null)
        {
            newSprite = enemy.GetCurrentSpriteData().ActiveSprite;
        }

        else if (button != null)
        {
            newSprite = button.GetCurrentSpriteData().ActiveSprite;
        }

        else if (exit != null)
        {
            newSprite = exit.GetCurrentSpriteData().ActiveSprite;
        }

        if (newSprite != null)
        {
            spriteRenderer.sprite = newSprite;
        }

    }
}
