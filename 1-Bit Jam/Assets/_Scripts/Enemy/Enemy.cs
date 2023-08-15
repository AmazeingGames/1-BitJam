using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IColored
{
    [field: SerializeField] public Sprites Sprites { get; private set; }

    [SerializeField] ColorSwap.Color startingColor;

    [SerializeField] bool showDebug;

    SpriteRenderer spriteRenderer;

    public ColorSwap.Color Color { get; private set; }
    public bool IsActiveProperty { get; private set; }

    void OnEnable()
    {
        SubscribeToColorSwap(true);
    }

    void OnDisable()
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

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        Color = startingColor;
    }

    public void HandleColorSwap(ColorSwap.Color newColor)
    {
        IsActiveProperty = IsActiveCheck(newColor);

        Sprite newSprite;

        if (showDebug)
            Debug.Log($"Set enemy sprite active : {IsActiveProperty}");

        if (IsActiveProperty)
        {
            newSprite = Sprites.ActiveSprite;
        }
        else
        {
            newSprite = Sprites.UnactiveSprite;
        }

        spriteRenderer.sprite = newSprite;
    }

    public bool IsActiveCheck(ColorSwap.Color backgroundColor) => Color != backgroundColor;
}
