using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Button Data")]
public class ButtonData : ScriptableObject
{
    [field: SerializeField] public float InteractRadius { get; private set; }

    [field: SerializeField] public bool IsConstant { get; private set; }


    [field: SerializeField] public ColorSwap.Color SwapColor { get; private set; }

    [field: SerializeField] public ColorSwap.Color ActiveColor { get; private set; }
}
