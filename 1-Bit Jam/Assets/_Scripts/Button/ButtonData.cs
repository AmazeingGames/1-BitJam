using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Button Data")]
public class ButtonData : ScriptableObject
{
    [field: SerializeField] public float InteractRadius { get; private set; }

    [field: SerializeField] public bool IsConstant { get; private set; }

    [field: SerializeField] public ColorSwap.Color Color { get; private set; }

    [field: SerializeField] public SpriteData SpriteData { get; private set; }
}
