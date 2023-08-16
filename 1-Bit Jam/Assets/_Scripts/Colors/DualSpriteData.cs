using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Sprites/Dual Sprite Data")]
public class DualSpriteData : SpriteData
{
    [field: Header("Light Sprites")]
    [field: SerializeField] public Sprite LightActiveSprite { get; private set; }
    [field: SerializeField] public Sprite LightInactiveSprite { get; private set; }

    [field: Header("Dark Sprites")]
    [field: SerializeField] public Sprite DarkActiveSprite { get; private set; }
    [field: SerializeField] public Sprite DarkInactiveSprite { get; private set; }


    public Sprite GetSprite(ColorSwap.Color color, bool isActive)
    {
        switch (color)
        {
            case ColorSwap.Color.White:
                if (isActive)
                    return LightActiveSprite;
                return LightInactiveSprite;

            case ColorSwap.Color.Black:
                if (isActive)
                    return DarkActiveSprite;
                return DarkInactiveSprite;

            case ColorSwap.Color.Neutral:
                if (isActive)
                    return DefaultActiveSprite;
                return DefaultInactiveSprite;

            default:
                Debug.LogWarning("Color not recognized");
                return null;
        }
    }
}
