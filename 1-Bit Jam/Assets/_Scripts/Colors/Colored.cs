using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Colored : MonoBehaviour
{
    public void OnEnable()
    {
        if (ColorSwap.Instance != null)
            ColorSwap.Instance.OnColorChange += HandleColorSwap;
    }

    public void OnDisable()
    {
        if (ColorSwap.Instance != null )
            ColorSwap.Instance.OnColorChange -= HandleColorSwap;
    }

    protected abstract void HandleColorSwap(ColorSwap.Color newColor);

    public abstract bool IsActiveCheck(ColorSwap.Color backgroundColor);
}
