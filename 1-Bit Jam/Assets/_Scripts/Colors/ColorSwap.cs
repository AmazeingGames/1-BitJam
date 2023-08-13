using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColorSwap : Singleton<ColorSwap>
{
    [SerializeField] bool showDebug = true;

    public enum Color { White, Black }

    Dictionary<Color, GameObject> colorToBackground;

    public Color BackgroundColor { get; private set; }

    public event Action <Color> OnColorChange;


    private void OnEnable()
    {
        if (showDebug)
            Debug.Log($"Is instance null : {Instance == null}");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeColor(Color.White);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeColor(Color.Black);
        }
    }

    void ChangeColor(Color newColor)
    {
        if (showDebug)
            Debug.Log($"Set background to {newColor}");

        BackgroundColor = newColor;

        OnColorChange?.Invoke(newColor);
    }
}
