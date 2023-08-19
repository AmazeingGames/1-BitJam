using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TileController : MonoBehaviour, IColored
{
    GameObject activeHeavenTiles;
    GameObject inactiveHeavenTiles;

    GameObject activeHellTiles;
    GameObject inactiveHellTiles;

    GameObject tileMaps;

    void OnEnable()
    {
        SubscribeToColorSwap(true);
    }

    void OnDisable()
    {
        SubscribeToColorSwap(false);
    }

    void Start()
    {
        tileMaps = GameObject.Find("TileMaps");

        activeHeavenTiles = tileMaps.transform.Find("Heaven Active").gameObject;
        inactiveHeavenTiles = tileMaps.transform.Find("Heaven Inactive").gameObject;
        activeHellTiles = tileMaps.transform.Find("Hell Active").gameObject;
        inactiveHellTiles = tileMaps.transform.Find("Hell Inactive").gameObject;


        SetTilesActive(ColorSwap.Instance.BackgroundColor);
    }

    void SubscribeToColorSwap(bool isSubscribing)
    {
        if (ColorSwap.Instance == null)
        {
            Debug.LogWarning("ColorSwap.Instance is null");
            return;
        }

        if (isSubscribing)
        {
            ColorSwap.Instance.OnColorChange += HandleColorSwap;
        }
        else
        {
            ColorSwap.Instance.OnColorChange -= HandleColorSwap;
        }
    }


    public void HandleColorSwap(ColorSwap.Color newColor)
    {
        SetTilesActive(newColor);
    }

    void SetTilesActive(ColorSwap.Color newColor)
    {
        if (newColor == ColorSwap.Color.White)
        {
            activeHeavenTiles.SetActive(true);
            inactiveHeavenTiles.SetActive(false);

            activeHellTiles.SetActive(false);
            inactiveHellTiles.SetActive(true);
        }
        else if (newColor == ColorSwap.Color.Black)
        {
            activeHellTiles.SetActive(true);
            inactiveHellTiles.SetActive(false);

            activeHeavenTiles.SetActive(false);
            inactiveHellTiles.SetActive(true);
        }
        else
            throw new NotImplementedException();
    }

    public bool IsActiveCheck(ColorSwap.Color backgroundColor)
    {
        throw new System.NotImplementedException();
    }
}
