using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugHelper
{
    public static void ShouldLog(string message, bool shouldLog)
    {
        if (shouldLog)
            Debug.Log(message);
    }
}
