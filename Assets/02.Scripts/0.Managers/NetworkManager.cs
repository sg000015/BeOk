using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;


public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instanceNW = null;
    string roomNum = "1";
    bool isOculus = false;
    public TMP_InputField inputRoomNum;

    void Awake()
    {
        if (instanceNW == null)
        {
            instanceNW = this;
            DontDestroyOnLoad(this.gameObject);
            Debug.Log("싱글톤 초기화");
        }
        else
        {
            Destroy(this);
            Debug.Log("싱글톤 존재해서 삭제");
        }
        Init();

    }
    void Start()
    {
        Connect();
    }
    public void Init()
    {

        if(0 == string.Compare(SystemInfo.deviceType.ToString(), "Desktop"))
        {
            isOculus = true;
            Debug.Log("오큘러스로 접속");
            GameObject.Find("Canvas-Phone").SetActive(false);

        }
        else
        {
            isOculus = false;
            Debug.Log("휴대폰으로 접속");
            GameObject.Find("Canvas-Oculus").SetActive(false);
            GameObject.Find("CurvedUILaserPointer").SetActive(false);

        }

    }
    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster()
    {
        Debug.Log("포톤 연결됨");
    }

    public void OnCreateRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (isOculus)
            {
                roomNum = "1";
            }
            else
            {
                roomNum =  GameObject.FindWithTag("LobbyInput").GetComponent<TMP_InputField>().text;
            }
            PhotonNetwork.JoinOrCreateRoom(roomNum, new Photon.Realtime.RoomOptions{ MaxPlayers = 2}, null);
        }
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("방생성 실패");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log(roomNum + "번 방 입장 완료");
        PhotonNetwork.LoadLevel("Ward-Injection");
    }

    public void InstantiatePlayer()
    {
        if(isOculus)
        {
            GameObject Player = PhotonNetwork.Instantiate("Player", new Vector3(-4f, 0, 1.6f), new Quaternion(0,0.7071068f,0,0.7071068f),0);
            Player.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        
        }
        else
        {
            GameObject CCTV = PhotonNetwork.Instantiate("CCTV",Vector3.zero, Quaternion.identity,0);
            CCTV.transform.GetChild(0).gameObject.SetActive(true);

        }
    }

    

}
