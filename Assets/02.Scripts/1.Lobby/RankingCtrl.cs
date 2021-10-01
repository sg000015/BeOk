using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankingCtrl : MonoBehaviour
{
    public GameObject NickNamePanel;
    public GameObject RankingPanel;
    public TMP_Text text;
    GameObject prevPanel;
    GameObject curPanel;

    public void OnClickKey(string str)
    {
        if (SW)
            text.text += str.ToUpper();
        else
            text.text += str.ToLower();

    }

    bool SW = false;
    public void OnClickShift()
    {
        Transform keyboard = NickNamePanel.transform.GetChild(0);
        SW = !SW;
        foreach (TMP_Text text in keyboard.GetComponentsInChildren<TMP_Text>())
        {
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

    public void OnClickDelete()
    {
        text.text = " ";
    }

    public void OnClickSpace()
    {
        text.text += " ";
    }

    public void OnClickEnterBtn()
    {
        //! Jumi
        InjectionMgr.injection.InsertData(text.text);

        prevPanel = NickNamePanel;
        curPanel = RankingPanel;
        ShowAndHideUI();

    }

    public void ShowAndHideUI(bool direction = true)
    {
        prevPanel.SetActive(!direction);
        curPanel.SetActive(direction);
    }
}
