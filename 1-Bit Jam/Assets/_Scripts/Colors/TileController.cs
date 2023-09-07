using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TileController : Colored
{
    GameObject activeHeavenTiles;
    GameObject inactiveHeavenTiles;

    GameObject activeHellTiles;
    GameObject inactiveHellTiles;

    GameObject tileMaps;

    bool needsToSetup = true;

    void Start()
    {
        GameManager.GameStart += HandleLevelLoad;
    }

    void HandleLevelLoad()
    {
        StartCoroutine(Setup());

        Debug.Log("Handled level load");
    }

    protected override void HandleColorSwap(ColorSwap.Color newColor)
    {
        SetTilesActive(newColor);
    }

    IEnumerator Setup()
    {
        Debug.Log("Setup");

        while (true)
        {
            yield return null;

            tileMaps = GameObject.Find("TileMaps");

            if (tileMaps != null)
                break;

            Debug.Log("waiting");
        }
        
        Debug.Log($"Are tileMaps null : {tileMaps == null}");

        for (int i = 0; i < tileMaps.transform.childCount; i++)
        {
            var child = tileMaps.transform.GetChild(i).gameObject;
            
            switch (child.name)
            {
                case "Heaven Active":
                    activeHeavenTiles = child;
                    break;

                case "Heaven Inactive":
                    inactiveHeavenTiles = child;
                    break;

                case "Hell Active":
                    activeHellTiles = child;
                    break;

                case "Hell Inactive":
                    inactiveHellTiles = child;
                    break;

                default:
                    throw new Exception("Name not recognized.");
            }
        }

        Debug.Log("TileMaps done");

        ColorSwap.Instance.ChangeColor(GameManager.Instance.LevelDataCurrent.StartingColor, gameObject, triggerAmbienceSounds: true);
        
        SetTilesActive(ColorSwap.Instance.BackgroundColor);

        yield break;
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
            inactiveHeavenTiles.SetActive(true);
        }
        else
            throw new NotImplementedException();
    }

    public override bool IsActiveCheck(ColorSwap.Color backgroundColor)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnStart()
    {
        throw new NotImplementedException();
    }
}
