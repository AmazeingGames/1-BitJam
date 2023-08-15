using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "Sprites")]
public class Sprites : ScriptableObject
{
    [field: SerializeField] public AnimatorController Controller { get; private set; }

    [field: SerializeField] public Sprite ActiveSprite { get; private set; }
    [field: SerializeField] public Sprite UnactiveSprite { get; private set; }



}
