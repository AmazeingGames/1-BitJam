using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : UIButtonBase
{
    [SerializeField] int levelToLoad;
    [SerializeField] List<Sprite> levelIcons;
    
    Image image;

    bool levelExists;

    void Start()
    {
        SetSprite();

        levelExists = GameManager.DoesLevelExist(levelToLoad);

        gameObject.SetActive(levelExists);

    }


    public override void OnClick()
    {
        base.OnClick();

        if (levelExists)
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.LevelStart, levelToLoad);
        }
        else
        {
            Debug.Log("Not Playable");
        }
        
    }

    public void SetSprite()
    {
        if (image == null)
            image = GetComponent<Image>();

        image.sprite = levelIcons[levelToLoad - 1];

    }

    public override void OnEnter()
    {

    }
}
