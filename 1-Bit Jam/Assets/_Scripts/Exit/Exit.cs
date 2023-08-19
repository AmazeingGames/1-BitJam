using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This, Button, and Enemy clearly share some repeating qualities.
//TO DO: Make changes to reduce repeated code.
public class Exit : MonoBehaviour, IColored
{
    [field: SerializeField] public ColorSwap.Color Color { get; private set; }

    [field: SerializeField] public SpriteData DarkExitData { get; private set; }
    [field: SerializeField] public SpriteData LightExitData { get; private set; }

    public SpriteData Sprites { get; private set; }

    [SerializeField] bool showDebug;

    SpriteRenderer spriteRenderer;
    ColoredAnimator exitAnimator;
    Animator animator;

    public bool IsActiveProperty { get; private set; }

    bool playPhaseAnimation;

    void OnEnable()
    {
        SubscribeToColorSwap(true);
    }

    void OnDisable()
    {
        SubscribeToColorSwap(false);
    }

    // Start is called before the first frame update
    void Awake()
    {
        Sprites = Color switch
        {
            ColorSwap.Color.White => LightExitData,
            ColorSwap.Color.Black => DarkExitData,
            ColorSwap.Color.Neutral => throw new NotImplementedException(),
            _ => throw new Exception(),
        };
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        exitAnimator = GetComponent<ColoredAnimator>();
        animator = GetComponent<Animator>();

        animator.runtimeAnimatorController = Sprites.Controller;
    }


    void Update()
    {
        if (!GameManager.Instance.IsLevelPlaying)
            return;

        CheckAnimations();
    }
    void CheckAnimations()
    {
        if (playPhaseAnimation)
        {
            Debug.Log("checked phase");
            exitAnimator.ShouldPlayPhaseIn(true, IsActiveProperty);
            playPhaseAnimation = false;
        }
        else
        {
            exitAnimator.ShouldPlayIdle(IsActiveProperty);
            exitAnimator.ShouldPlayInactive(IsActiveProperty);
        }
    }

    void SubscribeToColorSwap(bool isSubscribing)
    {
        if (ColorSwap.Instance == null)
        {
            Debug.LogWarning("ColorSwap.Instance is null");
            return;
        }

        if (isSubscribing)
        {
            ColorSwap.Instance.OnColorChange += HandleColorSwap;
        }
        else
        {
            ColorSwap.Instance.OnColorChange -= HandleColorSwap;
        }
    }

    public void HandleColorSwap(ColorSwap.Color newColor)
    {
        IsActiveProperty = IsActiveCheck(newColor);

        Sprite newSprite;

        DebugHelper.ShouldLog($"Set Exit sprite active : {IsActiveProperty}", showDebug);

        if (IsActiveProperty)
        {
            newSprite = Sprites.ActiveSprite;
        }
        else
        {
            newSprite = Sprites.InactiveSprite;
        }

        spriteRenderer.sprite = newSprite;

        playPhaseAnimation = true;
    }

    public bool IsActiveCheck(ColorSwap.Color backgroundColor)
    {
        if (Color == ColorSwap.Color.Neutral)
            return true;

        return Color != backgroundColor;
    }
}
