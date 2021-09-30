using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonMgr : MonoBehaviour
{
    public GameObject homePanel;
    public GameObject selectPanel;

    public GameObject NickNamePanel;
    public GameObject contentsPanel;
    public GameObject backButton;
    public GameObject ranksPanelI;

    public TMP_Text text;

    public GameManager_Lobby GM;



    GameObject prevPanel;
    GameObject curPanel;

    public void OnClickStartBtn()
    {
        prevPanel = homePanel;
        curPanel = selectPanel;
        ShowAndHideUI();
    }
    public void OnClickBackBtn()
    {
        prevPanel = selectPanel;
        curPanel = homePanel;
        ShowAndHideUI();
    }

    public void OnClickNickBtn()
    {
        text.text = " ";
        prevPanel = contentsPanel;
        curPanel = NickNamePanel;
        ShowAndHideUI();
    }

    public void OnClickNickBtnRancksI()
    {
        prevPanel = ranksPanelI;
        curPanel = contentsPanel;
        ShowAndHideUI();
    }

    public void OnClickRanksI()
    {
        prevPanel = contentsPanel;
        curPanel = ranksPanelI;
        ShowAndHideUI();
    }
    public void OnClickEnterBtn()
    {

        prevPanel = NickNamePanel;
        curPanel = contentsPanel;
        ShowAndHideUI();

        //!
        GM.SetNickname();

    }


    bool SW = false;
    public void OnClickShift()
    {
        Transform keyboard = NickNamePanel.transform.GetChild(0);
        SW = !SW;
        foreach (TMP_Text text in keyboard.GetComponentsInChildren<TMP_Text>())
        {
            Debug.Log(text.text);
            if (text.text == "Enter" ||
                text.text == "Space" ||
                text.text == "Delete")
            {

            }
            else
            {

                if (SW)
                {
                    text.text = text.text.ToUpper();
                }
                else
                {
                    text.text = text.text.ToLower();
                }
            }
        }
    }

    public void OnClickKey(string str)
    {
        if (SW)
            text.text += str.ToUpper();
        else
            text.text += str.ToLower();

    }
    public void OnClickDelete()
    {
        text.text = " ";
    }

    public void OnClickInjection()
    {
        SceneManager.LoadScene("Ward-Injection");
    }


    public void ShowAndHideUI(bool direction = true)
    {
        prevPanel.SetActive(!direction);
        curPanel.SetActive(direction);
    }
}
