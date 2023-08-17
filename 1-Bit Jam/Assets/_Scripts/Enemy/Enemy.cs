using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IColored
{
    [field: SerializeField] public SpriteData Sprites { get; private set; }

    [SerializeField] ColorSwap.Color color;

    [SerializeField] bool showDebug;

    SpriteRenderer spriteRenderer;
    EnemyAnimator enemyAnimator;
    Animator animator;

    public ColorSwap.Color Color { get; private set; }
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

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyAnimator = GetComponent<EnemyAnimator>();
        animator = GetComponent<Animator>();

        animator.runtimeAnimatorController = Sprites.Controller;

        Color = color;
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
            newSprite = Sprites.DefaultActiveSprite;
        }
        else
        {
            newSprite = Sprites.DefaultInactiveSprite;
        }

        spriteRenderer.sprite = newSprite;

        playPhaseAnimation = true;
    }

    public bool IsActiveCheck(ColorSwap.Color backgroundColor) => Color == backgroundColor;
}
