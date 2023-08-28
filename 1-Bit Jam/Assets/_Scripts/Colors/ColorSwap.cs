using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static AudioManager;
using static AudioManager;

public class ColorSwap : Singleton<ColorSwap>
{
    [SerializeField] bool showDebug = true;

    readonly List<GameObject> whiteListed = new();

    public enum Color { White, Black, Neutral, Null }

    public Color BackgroundColor { get; private set; }

    public event Action <Color> OnColorChange;


    private void OnEnable()
    {
        DebugHelper.ShouldLog($"Is instance null : {Instance == null}", showDebug);
    }

    void Start()
    {
        whiteListed.Add(gameObject);
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeColor(Color.White, gameObject);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeColor(Color.Black, gameObject);
        }
        */
    }

    public void ChangeColor(Color newColor, GameObject callingObject)
    {
        if (whiteListed.Contains(callingObject))
        {
            BackgroundColor = newColor;

            OnColorChange?.Invoke(newColor);

            AudioManager.Instance.PlayAudioClip(EventSounds.ColorSwap, callingObject.transform.position);

            float isHeaven = 0f;

            if (BackgroundColor == Color.White)
                isHeaven = 1f;

            AudioManager.Instance.SetAmbienceParameter(EventInstances.HeavenAmbience, "IsHeaven", isHeaven);
            AudioManager.Instance.SetAmbienceParameter(EventInstances.DevilishAmbience, "IsHeaven", isHeaven);
        }
        else
            Debug.LogWarning("Calling script doesn't have access to this function.");
    }

    public Color OppositeColor() => OppositeColor(BackgroundColor);

    public Color OppositeColor(Color contrastColor)
    {
        return contrastColor switch
        {
            Color.White => Color.Black,
            Color.Black => Color.White,
            _ => throw new Exception(),
        };
    }

    public void AddToWhiteList(GameObject gameObject) => whiteListed.Add(gameObject);
}
