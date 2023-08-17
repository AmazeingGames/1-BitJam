using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugHelper
{
    //Not sure if I like this approach; it obscures the original location of the debug
    public static void ShouldLog(string message, bool shouldLog)
    {
        if (shouldLog)
            Debug.Log(message);
    }
}
