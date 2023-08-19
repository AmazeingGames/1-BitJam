using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Sprite Data")]
public class SpriteData : ScriptableObject
{
    [field: SerializeField] public RuntimeAnimatorController Controller { get; private set; }

    [field: Header("DefaultSprite")]
    [field: SerializeField] public Sprite ActiveSprite { get; private set; }
    [field: SerializeField] public Sprite InactiveSprite { get; private set; }
}
