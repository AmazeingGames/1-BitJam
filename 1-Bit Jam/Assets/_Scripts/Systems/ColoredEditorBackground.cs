using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ColoredEditorBackground : MonoBehaviour
{
    ColorSwap.Color gameObjectColor;

    Background background;

    GameObject child;

    GameManager manager;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    #if UNITY_EDITOR
        if (Application.isPlaying)
            return;

        if (background == null)
        {
            background = GetComponent<Background>();
        }

        gameObjectColor = background.color;

        if (manager == null)
        {
            manager = GameObject.Find("GameStateManager").GetComponent<GameManager>();
        }

        if (child == null)
        {
            child = transform.GetChild(0).gameObject;
        }

        bool setActive = gameObjectColor == manager.LevelData.StartingColor;

        child.SetActive(setActive);
    #endif
    }
}
