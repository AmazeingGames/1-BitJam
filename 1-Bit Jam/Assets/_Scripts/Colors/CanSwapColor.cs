using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanSwapColor : MonoBehaviour
{
    private void Start()
    {
        ColorSwap.Instance.AddToWhiteList(gameObject);
    }
}
