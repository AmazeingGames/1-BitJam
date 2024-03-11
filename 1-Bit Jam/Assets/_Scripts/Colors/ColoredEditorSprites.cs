using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class ColoredEditorSprites : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] ColoredObject coloredObject;

    //Awake is only called when the script is instanced.
    void Awake()
        => runInEditMode = true;

    // Update is only called when something in the Scene changed.
    void Update()
    {
#if UNITY_EDITOR
        // This code, obviously would be drastically improved if both Enemy and Button inherited logic from the same script or if I could just grab SpriteData directly
        if (!Application.isPlaying)
        {
            if (coloredObject == null)
                return;

            Sprite newSprite = coloredObject.SpriteData.ActiveSprite;
            if (newSprite != null)
                spriteRenderer.sprite = newSprite;
        }
#endif
    }

    
}
