using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance = null;

    public bool isMenu = false;
    public Timer timer;
    public GameObject ExitMenu;
    public GameObject VrCamera;
    public TMPro.TMP_Text logText;
    public PhotonView pv;

    #region SINGLETON
    void Awake()
    {
        instance = this;
        NetworkManager.instanceNW.InstantiatePlayer();
        pv = this.gameObject.GetPhotonView();
    }

    #endregion

    private void Start()
    {
        // StartCoroutine(nameof(PrintLog));
    }

    private void Update()
    {
        // A 버튼 누름 && 메뉴 꺼져있음
        if (OVRInput.GetDown(OVRInput.Button.One) && !isMenu)
        {
            isMenu = true;

            // Timer Stop
            timer.timerOn = false;

            // UI On
            ExitMenu.SetActive(true);
        }

        // A 버튼 누름 && 메뉴 켜져있음
        else if (OVRInput.GetDown(OVRInput.Button.One) && isMenu)
        {
            // Go to Lobby
            // SceneManager.LoadScene("Lobby");
            pv.RPC("DeleteRoom", RpcTarget.AllViaServer);

        }

        // B 버튼 누름 && 메뉴 켜져있음
        if (OVRInput.GetDown(OVRInput.Button.Two) && isMenu)
        {
            isMenu = false;

            // Timer Start
            timer.timerOn = true;

            // UI Off
            ExitMenu.SetActive(false);
        }
    }

    public GameObject inDirector;
    public GameObject StartStatePanel;

     public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
       if (PhotonNetwork.CurrentRoom.PlayerCount > 0)
       {
           pv.RPC("StartUI", RpcTarget.AllViaServer);
       }
    }
    [PunRPC]
    void StartUI()
    {
        inDirector.SetActive(true);
        StartStatePanel.SetActive(false);
    }


    [ContextMenu("로비로 나가기")]
    public void OnClickOutToLobby()
    {
           pv.RPC("DeleteRoom", RpcTarget.AllViaServer);

    }
    [PunRPC]
    void DeleteRoom()
    {
        PhotonNetwork.DestroyAll();
        PhotonNetwork.LeaveRoom();

        if(0 == string.Compare(SystemInfo.deviceType.ToString(), "Desktop") )
        {
            PhotonNetwork.LoadLevel("Lobby");

        }
        else
        {
            PhotonNetwork.LoadLevel("Lobby");
        }
    }

    // IEnumerator PrintLog()
    // {
    //     yield return new WaitForSeconds(2.5f);

    //     GameObject ovrPlayer;

    //     try
    //     {
    //         // try 스택 영역에서 에러가 발생하면 catch 영역으로 넘어간다.
    //         ovrPlayer = GameObject.Find("ovrPlayer");
    //     }
    //     catch (NullReferenceException nr)
    //     {
    //         // 에러가 발생하면 에러 내용을 콘솔 출력한다.
    //         logText.text += "플레이어 없음 1\n";
    //     }
    //     catch (Exception e)
    //     {
    //         // 에러가 발생하면 에러 내용을 콘솔 출력한다.
    //         logText.text += "플레이어 없음 2\n";
    //     }
    //     finally
    //     {
    //         logText.text += "로그 찍히는 중";
    //     }
    // }



}
