using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Yeah, this is very clearly silly now but I don't want to change it again
public class ColoredObject : ScriptableObject
{
    [field: SerializeField] public SpriteData SpriteData { get; private set; }
}
