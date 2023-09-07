using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class ColoredEditorSprites : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    ColoredObject coloredObject;

    //Awake is only called when the script is instanced.
    void Awake()
    {
        #if UNITY_EDITOR
        runInEditMode = true;

        spriteRenderer = GetComponent<SpriteRenderer>();

        coloredObject = GetComponent<ColoredObject>();

        Debug.Log($"Colored object null : {coloredObject == null}");

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
        if (coloredObject == null)
            return;

        Sprite newSprite = coloredObject.GetCurrentSpriteData().ActiveSprite;

        if (newSprite != null)
            spriteRenderer.sprite = newSprite;
    }
}
