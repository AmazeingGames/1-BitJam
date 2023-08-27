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
        GameManager.OnStateEnter += HandleLevelLoad;
    }

    void HandleLevelLoad(GameManager.GameState gameState)
    {
        if (gameState != GameManager.GameState.LevelStart)
            return;

        StartCoroutine(Setup());

        Debug.Log("Handled level load");
    }

    protected override void HandleColorSwap(ColorSwap.Color newColor)
    {
        SetTilesActive(newColor);
    }

    IEnumerator Setup()
    {
        //Gives time to unload the current level
        yield return null;

        //Gives time to load the next level
        yield return new WaitWhile(IsTileMapsNull);

        tileMaps = GameObject.Find("TileMaps");

        activeHeavenTiles = tileMaps.transform.Find("Heaven Active").gameObject;
        inactiveHeavenTiles = tileMaps.transform.Find("Heaven Inactive").gameObject;
        activeHellTiles = tileMaps.transform.Find("Hell Active").gameObject;
        inactiveHellTiles = tileMaps.transform.Find("Hell Inactive").gameObject;

        ColorSwap.Instance.ChangeColor(GameManager.Instance.LevelDataCurrent.StartingColor, gameObject);
        
        SetTilesActive(ColorSwap.Instance.BackgroundColor);

        yield break;
    }

    bool IsTileMapsNull()
    {
        bool isNull = GameObject.Find("TileMaps") == null;

        if (isNull)
        {
            Debug.Log("waiting");
        }

        return isNull;
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
