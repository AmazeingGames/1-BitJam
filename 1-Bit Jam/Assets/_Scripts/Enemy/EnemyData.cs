using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    [field: SerializeField] public SpriteData SpriteData { get; private set; }
    [field: SerializeField] public ColorSwap.Color Color { get; private set; }
}
