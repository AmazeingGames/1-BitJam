using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour, IColored
{
    [SerializeField] GameObject backgroundObject;
    [SerializeField] ColorSwap.Color color;

    [SerializeField] bool showDebug = true;

    private void OnEnable()
    {
        SubscribeToColorSwap(true);
    }

    private void OnDisable()
    {
        SubscribeToColorSwap(false);
    }

    void SubscribeToColorSwap(bool isSubscribing)
    {
        if (ColorSwap.Instance == null)
        {
            if (showDebug)
                Debug.LogWarning("ColorSwap.Instance is null");
            return;
        }

        if (isSubscribing)
            ColorSwap.Instance.OnColorChange += HandleColorSwap;
        else
            ColorSwap.Instance.OnColorChange -= HandleColorSwap;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetSize();
    }

    void SetSize() => backgroundObject.transform.localScale = new Vector3(1000, 1000);

    public void HandleColorSwap(ColorSwap.Color newColor)
    {
        bool setActive = IsActiveCheck(newColor);

        if (showDebug)
            Debug.Log($"Set {newColor} background {setActive}");

        backgroundObject.SetActive(setActive);
    }

    public bool IsActiveCheck(ColorSwap.Color backgroundColor) => backgroundColor == color;
}
