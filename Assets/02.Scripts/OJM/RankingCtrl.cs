using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
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

    public enum SKILL { Injection, BloodCollection };
    public SKILL skillName = SKILL.Injection;

    public PhotonView pv;

    void Awake()
    {
        pv = gameObject.GetPhotonView();
    }
    public void OnClickKey(string str)
    {
        pv.RPC(nameof(OnClickKeyRPC), RpcTarget.AllViaServer, str);
    }
    [PunRPC]
    void OnClickKeyRPC(string str)
    {
        if (SW)
            text.text += str.ToUpper();
        else
            text.text += str.ToLower();
    }

    bool SW = false;
    public void OnClickShift()
    {
        pv.RPC(nameof(OnClickShiftRPC), RpcTarget.AllViaServer);
    }

    [PunRPC]
    void OnClickShiftRPC()
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
        pv.RPC(nameof(OnClickDeleteRPC), RpcTarget.AllViaServer);
    }
    [PunRPC]
    void OnClickDeleteRPC()
    {
        text.text = " ";

    }

    public void OnClickSpace()
    {
        pv.RPC(nameof(OnClickSpaceRPC), RpcTarget.AllViaServer);
    }
    [PunRPC]
    void OnClickSpaceRPC()
    {
        text.text += " ";
    }

    [ContextMenu("엔터")]
    public void OnClickEnterBtn()
    {

        // prevPanel = NickNamePanel;
        // curPanel = RankingPanel;
        // ShowAndHideUI();
        pv.RPC(nameof(OnClickEnterBtnRPC), RpcTarget.AllViaServer);


    }
    [PunRPC]
    void OnClickEnterBtnRPC()
    {
        NickNamePanel.SetActive(false);
        switch (skillName)
        {
            case SKILL.Injection:
                InjectionMgr.injection.InsertData(text.text);
                break;
            case SKILL.BloodCollection:
                GameObject.Find("UIManager").GetComponent<UIManager_blood>().InsertData(text.text);
                break;
        }
    }
    public void ShowAndHideUI(bool direction = true)
    {
        pv.RPC(nameof(ShowAndHideUIRPC), RpcTarget.AllViaServer);
    }
    [PunRPC]
    void ShowAndHideUIRPC()
    {
        prevPanel.SetActive(false);
        curPanel.SetActive(true);
    }

    public void ChangeCanvas()
    {

        pv.RPC(nameof(ChangeCanvasRPC), RpcTarget.AllViaServer);

    }
    [PunRPC]
    void ChangeCanvasRPC()
    {
        basicCanvas.SetActive(false);
        rankingCanvas.SetActive(true);
        GameObject.Find("Player(Clone)").transform.Find("CurvedUILaserPointer").gameObject.SetActive(true);
        GameObject.Find("LeftHandAnchor").transform.Find("LaserBeam")?.gameObject.SetActive(true);
        GameObject.Find("RightHandAnchor").transform.Find("LaserBeam")?.gameObject.SetActive(true);
    }

    public void ActiveNickname()
    {
        pv.RPC(nameof(ActiveNicknameRPC), RpcTarget.AllViaServer);
    }
    [PunRPC]
    void ActiveNicknameRPC()
    {
        NickNamePanel.SetActive(true);
    }

    public void InactiveNickname()
    {
        pv.RPC(nameof(InactiveNicknameRPC), RpcTarget.AllViaServer);
    }
    [PunRPC]
    void InactiveNicknameRPC()
    {
        NickNamePanel.SetActive(false);

    }

    public void ActiveRankingUI()
    {
        pv.RPC(nameof(ActiveRankingUIRPC), RpcTarget.AllViaServer);

    }
    [PunRPC]
    public void ActiveRankingUIRPC()
    {
        RankingPanel.SetActive(true);

    }

}
