using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;


public class NetworkManagerVR : MonoBehaviourPunCallbacks
{
    string roomNum = "1";

    void Start() => Connect();
    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster()
    {
        Debug.Log("연결됨");
        PhotonNetwork.CreateRoom(roomNum);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(roomNum + "번 방 생성");

    }

}
