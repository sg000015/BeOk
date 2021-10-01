using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RankingCtrl : MonoBehaviour
{
    public GameObject basicCanvas;
    public GameObject rankingCanvas;
    public GameObject LaserPoint;
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

    [ContextMenu("엔터")]
    public void OnClickEnterBtn()
    {

        // prevPanel = NickNamePanel;
        // curPanel = RankingPanel;
        // ShowAndHideUI();
        NickNamePanel.SetActive(false);

        Debug.Log($"닉네임패널 액티브 유무 : {NickNamePanel.activeSelf}");
        Debug.Log("엔터키를 누름");
        InjectionMgr.injection.InsertData(text.text);
    }

    public void ShowAndHideUI(bool direction = true)
    {
        prevPanel.SetActive(!direction);
        curPanel.SetActive(direction);
    }

    public void ChangeCanvas()
    {
        basicCanvas.SetActive(false);
        rankingCanvas.SetActive(true);
        LaserPoint.SetActive(true);
    }

    public void ActiveNickname()
    {
        NickNamePanel.SetActive(true);
    }

    public void ActiveRanking()
    {
        RankingPanel.SetActive(true);
    }
}
