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
                {
                    //Debug.Log("White, Active");
                    return LightActiveSprite;
                }
                //Debug.Log("White, Inactive");

                return LightInactiveSprite;

            case ColorSwap.Color.Black:
                if (isActive)
                {
                    //Debug.Log("Black, Active");
                    return DarkActiveSprite;
                }
                //Debug.Log("Black, Inactive");
                return DarkInactiveSprite;

            case ColorSwap.Color.Neutral:
                if (isActive)
                {
                    //Debug.Log("Neutral, Active");
                    return DefaultActiveSprite;
                }
               //Debug.Log("Neutral, Inactive");
                return DefaultInactiveSprite;

            default:
                //Debug.LogWarning("Color not recognized");
                return null;
        }
    }
}
