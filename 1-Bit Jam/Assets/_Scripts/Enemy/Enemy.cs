using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IColored
{
    public ColorSwap.Color Color { get; private set; }

    [SerializeField] Sprites sprites;
    [SerializeField] ColorSwap.Color startingColor;

    [SerializeField] bool showDebug;

    SpriteRenderer spriteRenderer;


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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleColorSwap(ColorSwap.Color newColor)
    {
        bool isActive = IsActive(newColor);

        Sprite newSprite;

        if (showDebug)
            Debug.Log($"Set enemy sprite active : {isActive}");

        if (isActive)
        {
            newSprite = sprites.ActiveSprite;
        }
        else
        {
            newSprite = sprites.UnactiveSprite;
        }

        spriteRenderer.sprite = newSprite;
    }

    public bool IsActive(ColorSwap.Color backgroundColor) => Color != backgroundColor;
}
