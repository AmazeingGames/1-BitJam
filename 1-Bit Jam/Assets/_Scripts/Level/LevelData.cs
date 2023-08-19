using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Data")]
public class LevelData : ScriptableObject
{

    [field: SerializeField] public ColorSwap.Color StartingColor { get; private set; }

    [field: SerializeField] public int Level { get; private set; }

}
