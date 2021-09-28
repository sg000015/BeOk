using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGB_WaitingRoomBtnCtrl : MonoBehaviour
{
    public GameObject homePanel;
    public GameObject selectPanel;

    public GameObject optionsPanel;
    public GameObject backButton;
    
    enum UISTATE 
    {
        Home,
        SelectMode,
        Options
    }
    
    UISTATE prevUIState = UISTATE.Home;
    public void OnClick(string str)
    {
        switch(str)
        {
            case "SelectMode" :
                InSelectMode();
                break;
            case "ChallengeMode" :
                break;
            case "Options" :
                InOptionsMode();
                break;
            case "1" :
                break;
            case "2" :
                break;
            case "back" :
                InHome();
                break;
        }        
    }

    void InSelectMode()
    {
        SetOffPrevUI();

        selectPanel.gameObject.SetActive(true);
        prevUIState = UISTATE.SelectMode;
        
    }
    void InHome()
    {
        SetOffPrevUI();

        homePanel.gameObject.SetActive(true);
        prevUIState = UISTATE.Home;
    }
    void InOptionsMode()
    {
        SetOffPrevUI();
        optionsPanel.gameObject.SetActive(true);
        prevUIState = UISTATE.Options;
    }

    void SetOffPrevUI()
    {
        if (prevUIState == UISTATE.Home)
        {
            backButton.gameObject.SetActive(true);
        }
        else
        {
            backButton.gameObject.SetActive(false);
        }

        if (prevUIState == UISTATE.Home)
        {
            homePanel.gameObject.SetActive(false);
        }
        else if(prevUIState == UISTATE.SelectMode)
        {
            selectPanel.gameObject.SetActive(false);
        }
        else if (prevUIState == UISTATE.Options)
        {
            optionsPanel.gameObject.SetActive(false);
        }
    }
}
