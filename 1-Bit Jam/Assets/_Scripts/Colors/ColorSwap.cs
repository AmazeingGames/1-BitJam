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

    //This function is called whenever we change the color of the world.
    //This is called at the start of every level for setup and everytime we push a button to change the world during gameplay.
    //It accepts two arguments, data which is given every time the function is called. 
    //newColor is a variable of type Color and it's used to determine which color to change the world to
    //callingObject is a variable of type GameObject and it's used to determine the origin of the sound and whether or not it has access to call this function
    public void ChangeColor(Color newColor, GameObject callingObject)
    {
        //Here we check if the object calling the ChangeColor function is allowed to do so
        //For example, we want only buttons and switches to change the color of the world, but not the exit or platforms. 
        //This is to help prevent errors while programming.
        if (whiteListed.Contains(callingObject))
        {
            //First it updates a variable, BackgroundColor, which is used to check the current color of the world
            BackgroundColor = newColor;

            //Then we notify other objects that the world has changed color so they can respond
            //For example, doors and enemies respond by phasing in and out of the level
            OnColorChange?.Invoke(newColor);

            //This statement, called a switch statement, looks at the given argument, and runs different code depending on its value
            switch (newColor)
            {
                //This code is saying, if we're swapping to the color white, which is Heaven, we're going to enter the code inside of the case
                case Color.White:
                    //Now, all the clip related to transferring to heaven will play

                    //This code will play the one shot for the transferring to HeavenSound
                    AudioManager.Instance.TriggerAudioClip(EventSounds.SwapToHeaven, callingObject);
                    break;

                //Otherwise, If we're swapping to the color black, or Hell, we're going to enter the code inside of this case instead 
                case Color.Black:
                    //Then this code will run, playing the HellSwap audio clip trigger
                    AudioManager.Instance.TriggerAudioClip(EventSounds.SwapToHell, callingObject);
                    break;

                //However, if we're swapping to something not listed in the previous cases, i.e. neither Heaven nor Hell, this code will run
                default:
                    //In which case, I have not clue what is happening and we're going to throw an error, known as an exception, so I can see what went wrong
                    throw new NotImplementedException();
            }
        }
        //If the calling object doesn't have access to this function (isn't white-listed) then only this code will run, notifying me that an object that's not supposed to, is attempting to access this function
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
