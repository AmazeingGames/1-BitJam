using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Button : MonoBehaviour, IColored
{
    [field: SerializeField] public ButtonData ButtonData { get; private set; }
    public SpriteData SpriteData { get => ButtonData.SpriteData; }

    [SerializeField] bool showDebug;

    Vision vision;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        vision = GetComponent<Vision>();
    }

    void Update()
    {
        if (vision.VisibleTargets.Contains(Player.Instance.Collider))
        {
            DebugHelper.ShouldLog("Player is within interact range", showDebug);
        }
    }

    private void OnEnable()
    {
        SubscribeToColorSwap(true);
    }

    private void OnDisable()
    {
        SubscribeToColorSwap(false);
    }

    void SubscribeToColorSwap(bool isSubscribing)
    {
        if (ColorSwap.Instance == null)
        {
            Debug.LogWarning("ColorSwap.Instance is null");
            return;
        }

        if (isSubscribing)
            ColorSwap.Instance.OnColorChange += HandleColorSwap;
        else
            ColorSwap.Instance.OnColorChange -= HandleColorSwap;
    }

    public bool IsActiveProperty { get; private set; }

    public void HandleColorSwap(ColorSwap.Color newColor)
    {
        IsActiveProperty = IsActiveCheck(newColor);

        spriteRenderer.sprite = SpriteData.ActiveSprite;
    }

    public bool IsActiveCheck(ColorSwap.Color backgroundColor)
    {
        if (ButtonData.Color == ColorSwap.Color.Neutral)
            return true;

        return (backgroundColor == ButtonData.Color);
    }
}
