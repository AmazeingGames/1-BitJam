using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//This and Button clearly share some repeating qualities. Could do something; not sure.
public class Enemy : MonoBehaviour, IColored
{
    [field: SerializeField] public ColorSwap.Color Color { get; private set; }

    [field: SerializeField] public EnemyData DarkEnemyData { get; private set; }
    [field: SerializeField] public EnemyData LightEnemyData { get; private set; }

    public EnemyData EnemyData { get; private set; }

    public SpriteData Sprites { get => EnemyData.SpriteData; }

    [SerializeField] bool showDebug;

    SpriteRenderer spriteRenderer;
    ColoredAnimator enemyAnimator;
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
    
    void Awake()
    {
        EnemyData = Color switch
        {
            ColorSwap.Color.White => LightEnemyData,
            ColorSwap.Color.Black => DarkEnemyData,
            ColorSwap.Color.Neutral => throw new NotImplementedException(),
            _ => throw new Exception(),
        };
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyAnimator = GetComponent<ColoredAnimator>();
        animator = GetComponent<Animator>();

        animator.runtimeAnimatorController = Sprites.Controller;
    }

    void Update()
    {
        CheckAnimations();
    }

    void CheckAnimations()
    {
        if (playPhaseAnimation)
        {
            Debug.Log("checked phase");
            enemyAnimator.ShouldPlayPhaseIn(true, IsActiveProperty);
            playPhaseAnimation = false;
        }
        else
        {
            enemyAnimator.ShouldPlayIdle(IsActiveProperty);
            enemyAnimator.ShouldPlayInactive(IsActiveProperty);
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

        DebugHelper.ShouldLog($"Set enemy sprite active : {IsActiveProperty}", showDebug);

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

        return Color == backgroundColor;
    }
}
