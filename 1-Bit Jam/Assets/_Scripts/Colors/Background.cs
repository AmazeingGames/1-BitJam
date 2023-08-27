using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : Colored
{
    [SerializeField] GameObject backgroundObject;
    [field: SerializeField] public ColorSwap.Color color { get; private set; }

    [SerializeField] bool showDebug = true;

    // Start is called before the first frame update
    void Start()
    {
        SetSize();
    }

    void SetSize() => backgroundObject.transform.localScale = new Vector3(1000, 1000);

    protected override void HandleColorSwap(ColorSwap.Color newColor)
    {
        bool setActive = IsActiveCheck(newColor);

        DebugHelper.ShouldLog($"Set {newColor} background {setActive}", showDebug);

        backgroundObject.SetActive(setActive);
    }

    public override bool IsActiveCheck(ColorSwap.Color backgroundColor) => backgroundColor == color;

    protected override void OnStart()
    {
        throw new System.NotImplementedException();
    }
}
