using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Colored : MonoBehaviour
{
    public void OnEnable()
    {
        SubscribeToColorSwap(true);
    }

    public void OnDisable()
    {
        SubscribeToColorSwap(false);
    }

    public abstract void HandleColorSwap(ColorSwap.Color newColor);

    public abstract bool IsActiveCheck(ColorSwap.Color backgroundColor);

    public void SubscribeToColorSwap(bool isSubscribing)
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
}
