using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainMenu : Singleton<MainMenu>
{
    [SerializeField] Canvas mainMenu;
    [SerializeField] Canvas levelSelect;

    public enum MenuState { MainMenu, LevelSelectMenu }

    public MenuState CurrentState { get; private set; }

    void OnEnable()
    {
        MainMenuBackButton.OnBack += HandleOnBack;
    }

    void OnDisable()
    {
        MainMenuBackButton.OnBack -= HandleOnBack;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdatState(MenuState.MainMenu);
    }

    void HandleOnBack()
    {
        Debug.Log("Pressed back");

        UpdatState(MenuState.MainMenu);
    }

    public void UpdatState(MenuState newState)
    {
        //Not sure if this is an issue, but this is always called the first time we exit a state, even when we haven't technically 'left' any states. This also happens in the main game manager.
        switch (CurrentState)
        {
            case MenuState.MainMenu:
                OnMainMenuExit();
                break;

            case MenuState.LevelSelectMenu:
                OnLevelSelectMenuExit();
                break;
        }
        
        CurrentState = newState;

        //Enter
        switch (newState)
        {
            case MenuState.MainMenu:
                OnMainMenuEnter();
                break;

            case MenuState.LevelSelectMenu:
                OnLevelSelectMenuEnter();
                break;
        }
    }

    void OnMainMenuEnter()
    {
        mainMenu.gameObject.SetActive(true);
        levelSelect.gameObject.SetActive(false);
    }

    void OnMainMenuExit()
    {
        mainMenu.gameObject.SetActive(false);
    }

    void OnLevelSelectMenuEnter()
    {
        levelSelect.gameObject.SetActive(true);
    }

    void OnLevelSelectMenuExit()
    {
        levelSelect.gameObject.SetActive(false);
    }
}
