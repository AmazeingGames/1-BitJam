using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static AudioManager;

public class ColorSwap : Singleton<ColorSwap>
{
    [SerializeField] bool showDebug = true;

    readonly List<GameObject> whiteListed = new();

    public enum Color { White, Black, Neutral, Null }
    public Color BackgroundColor { get; private set; }

    public event Action <Color> OnColorChange;


    private void OnEnable()
        => DebugHelper.ShouldLog($"Is instance null : {Instance == null}", showDebug);

    void Start()
        => whiteListed.Add(gameObject);
    
    void Update()
    {
    #if DEBUG
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeColor(Color.White, gameObject);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeColor(Color.Black, gameObject);
    #endif
    }

    // Changes the color of the world and notifies listeners
    public static void ChangeColor(Color newColor, GameObject callingObject, bool triggerSwapSounds = false, bool triggerAmbienceSounds = false)
    {
        Manager.InstanceNullCheck();

        if (Instance.whiteListed.Contains(callingObject))
        {
            Instance.BackgroundColor = newColor;

            Instance.OnColorChange?.Invoke(newColor);

            EventSounds eventToTrigger;

            eventToTrigger = newColor switch
            {
                Color.White when triggerSwapSounds      => EventSounds.SwapToHeaven,
                Color.White when triggerAmbienceSounds  => EventSounds.HeavenAmbience,

                Color.Black when triggerSwapSounds      => EventSounds.SwapToHell,
                Color.Black when triggerAmbienceSounds  => EventSounds.DevilishAmbience,
                _                                       => EventSounds.Null,
            };

            AudioManager.Instance.TriggerAudioClip(eventToTrigger, callingObject);
        }
        else
            Debug.LogWarning("Calling script doesn't have access to this function.");
    }

    // Returns the color opposite to the given color
    public static Color OppositeColor(Color contrastColor)
    {
        return contrastColor switch
        {
            Color.White => Color.Black,
            Color.Black => Color.White,
            _ => throw new Exception(),
        };
    }

    // Returns the color opposite to the background
    public static Color OppositeColor()
    {
        Manager.InstanceNullCheck();

        return OppositeColor(Instance.BackgroundColor);
    }

    // Makes sure only whitelisted objs can change world color
    public static IEnumerator AddToWhiteList(GameObject gameObject) 
    {
        Manager.InstanceNullCheck();

        yield return null;
        
        Instance.whiteListed.Add(gameObject);

    }
}
