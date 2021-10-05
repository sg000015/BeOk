using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;


public class NetworkManagerPhone : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomNumber;
    public TMP_Text info;

    // void Start() => Connect();
    // public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public void OnClickEnter()
    {
        PhotonNetwork.ConnectUsingSettings();
        info.text = roomNumber.text +"방에 입장 중입니다.";
        Invoke("TryJoinRoom", 1f);
    }
    void TryJoinRoom()
    {
        PhotonNetwork.JoinRoom(roomNumber.text);
        // PhotonNetwork.JoinOrCreateRoom(roomNumber.text, new Photon.Realtime.RoomOptions{ MaxPlayers = 2}, null);

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        info.text = "존재하지 않는 방입니다";
    }

    public override void OnJoinedRoom()
    {
        info.text = "방 입장에 성공하였습니다";
        Invoke("SceneLoader", 1.5f);
    }

    void SceneLoader()
    {
        SceneManager.LoadScene("BGBWard-Injection");
    }
}
