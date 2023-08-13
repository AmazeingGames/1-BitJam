using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IColored
{
    void HandleColorSwap(ColorSwap.Color newColor);

    bool IsActive(ColorSwap.Color backgroundColor);
}
