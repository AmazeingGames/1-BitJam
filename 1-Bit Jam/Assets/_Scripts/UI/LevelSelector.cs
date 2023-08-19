using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LevelSelector : UIButtonBase
{
    [SerializeField] bool isPlayable = true;
    [SerializeField] int levelToLoad;
    [SerializeField] List<Sprite> levelIcons;
    
    Image image;

    void Start()
    {
        image = GetComponent<Image>();

        gameObject.SetActive(isPlayable);

        image.sprite = levelIcons[levelToLoad - 1];
    }

    public override void OnClick()
    {
        if (isPlayable)
        {
            Debug.Log($"Loaded level {levelToLoad}");

            GameManager.Instance.LoadLevel(levelToLoad);
        }
        else
        {
            Debug.Log("Not");
        }
        
    }

    public override void OnEnter()
    {

    }
}
