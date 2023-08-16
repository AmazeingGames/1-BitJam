using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(menuName = "Sprites/Sprite Data")]
public class SpriteData : ScriptableObject
{
    [field: SerializeField] public AnimatorController Controller { get; private set; }

    [field: Header("DefaultSprite")]
    [field: SerializeField] public Sprite DefaultActiveSprite { get; private set; }
    [field: SerializeField] public Sprite DefaultInactiveSprite { get; private set; }



}
