using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LevelSelectorEditorSprite : MonoBehaviour
{
    LevelSelector levelSelector;

    // Start is called before the first frame update
    void Start()
    {
        levelSelector = GetComponent<LevelSelector>();

        levelSelector.SetSprite();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
