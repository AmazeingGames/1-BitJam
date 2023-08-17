using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Object Data/Enemy Data")]
//This should inherit from a base scriptable object class. Maybe 'ObjectData' or 'ColoredData'
public class EnemyData : ScriptableObject
{
    [field: SerializeField] public ColorSwap.Color Color { get; private set; }
    [field: SerializeField] public SpriteData SpriteData { get; private set; }
}
