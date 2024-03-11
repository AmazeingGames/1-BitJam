using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : Colored
{
    [SerializeField] GameObject backgroundObject;
    [SerializeField] bool showDebug = true;
    [field: SerializeField] public ColorSwap.Color Color { get; private set; }

    // Start is called before the first frame update
    void Start()
        => backgroundObject.transform.localScale = new Vector3(1000, 1000);

    protected override void HandleColorSwap(ColorSwap.Color newColor)
    {
        bool setActive = IsActiveCheck(newColor);

        DebugHelper.ShouldLog($"Set {newColor} background {setActive}", showDebug);

        backgroundObject.SetActive(setActive);
    }

    public override bool IsActiveCheck(ColorSwap.Color backgroundColor) => backgroundColor == Color;
}
