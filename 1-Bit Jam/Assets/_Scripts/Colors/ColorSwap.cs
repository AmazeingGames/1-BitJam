using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColorSwap : Singleton<ColorSwap>
{
    [SerializeField] bool showDebug = true;

    public enum Color { White, Black, Neutral }

    public Color BackgroundColor { get; private set; }

    public event Action <Color> OnColorChange;


    private void OnEnable()
    {
        DebugHelper.ShouldLog($"Is instance null : {Instance == null}", showDebug);
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
        DebugHelper.ShouldLog($"Set background to {newColor}", showDebug);

        BackgroundColor = newColor;

        OnColorChange?.Invoke(newColor);
    }
}
