using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Threading.Tasks;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instanceNW = null;
    string roomNum = "1";
    public string roomType = "";
    public bool isOculus = false;
    public TMP_InputField inputRoomNum;
    public TMP_Text text;

    public TMP_Text deviceCheckText;
    public Button phoneBtn;

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
            Destroy(this.gameObject);
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

        //! 빌드시 오큘러스이름 = Handheld, 핸드폰 이름도 동일

        if (0 == string.Compare(SystemInfo.deviceType.ToString(), "Handheld123")
            || 0 == string.Compare(SystemInfo.deviceType.ToString(), "Desktop11"))
        {
            isOculus = true;
        }
        else
        {
            text = GameObject.Find("NetworkInfoText").GetComponent<TMP_Text>();
            // Button btn = GameObject.Find("Button-OK").GetComponent<Button>();
            // Debug.Log(btn);
            // btn.onClick.AddListener(() => OnCreateRoom());
            isOculus = false;

            text.text = "감독관 모드로 접속";
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
                PhotonNetwork.CreateRoom(roomNum, new Photon.Realtime.RoomOptions { MaxPlayers = 2 }, null);
                Debug.Log("방만들기 시도");

            }
            else
            {
                Debug.Log("oncreateroon - phpone");
                roomNum = GameObject.FindWithTag("LobbyInput").GetComponent<TMP_InputField>().text;
                text = GameObject.Find("NetworkInfoText").GetComponent<TMP_Text>();
                text.text = roomNum + "번 방에 입장을 시도합니다.";

                PhotonNetwork.JoinRoom(roomNum);
                // PhotonNetwork.JoinOrCreateRoom(roomNum, new Photon.Realtime.RoomOptions { MaxPlayers = 2 }, null);
            }
        }
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("방생성 실패");

    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        text.text = "존재하지 않거나, 입장할 수 없는 방입니다.";
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("방에 접속");

        if (isOculus)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { { "SkillType", roomType } });
            // Debug.Log(PhotonNetwork.CurrentRoom.CustomProperties["SkillType"]);
            // Debug.Log("AAAAA");

            if (roomType == "Blood")
            {
                PhotonNetwork.LoadLevel("Ward-BloodCollection-BACKUP");
            }
            else
            {
                PhotonNetwork.LoadLevel("Ward-Injection");
            }
        }
        else
        {
            text.text = "방에 입장 중입니다.";
            string skill = PhotonNetwork.CurrentRoom.CustomProperties["SkillType"].ToString();
            if (skill == "Blood")
            {
                PhotonNetwork.LoadLevel("Ward-BloodCollection-BACKUP");
            }
            else
            {
                PhotonNetwork.LoadLevel("Ward-Injection");
            }
        }

        // if (roomType == "Blood")
        // {
        //     if (isOculus)
        //     {

        //     }
        //     if (!isOculus)
        //     {
        //         text.text = roomNum + "성공적으로 입장하였습니다.";
        //         text.text = roomNum + "씬을 전환합니다.";
        //     }
        //     PhotonNetwork.LoadLevel("Ward-BloodCollection-BACKUP");
        // }
        // else
        // {
        //     PhotonNetwork.LoadLevel("Ward-Injection");
        // }

    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Lobby");
    }



    public void InstantiatePlayer()
    {
        StartCoroutine(nameof(CreatePlayer));
    }

    IEnumerator CreatePlayer()
    {

        yield return new WaitUntil(() => PhotonNetwork.CurrentRoom.CustomProperties["SkillType"] != null);

        Debug.Log("teCreatePlayermp------");
        Debug.Log(PhotonNetwork.CurrentRoom.CustomProperties["SkillType"]);

        string skill = PhotonNetwork.CurrentRoom.CustomProperties["SkillType"].ToString();

        if (isOculus)
        {
            Vector3 pos = roomType == "Blood" ? new Vector3(-0.141f, 1.5f, -0.916f) : new Vector3(-5.6f, 1.5f, 1.6f);
            Quaternion rot = roomType == "Blood" ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 90, 0);

            GameObject Player = PhotonNetwork.Instantiate("Player", pos, rot, 0);
            Player.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            Player.transform.Find("EventSystem").gameObject.SetActive(true);
            Player.transform.Find("CurvedUILaserPointer").gameObject.SetActive(true);
            GameObject.Find("Canvas-Phone").SetActive(false);

        }
        else
        {

            GameObject CCTV = (skill == "Blood") ? PhotonNetwork.Instantiate("CCTV2", Vector3.zero, Quaternion.identity, 0)
                                                  : PhotonNetwork.Instantiate("CCTV", Vector3.zero, Quaternion.identity, 0);

            CCTV.transform.GetChild(0).gameObject.SetActive(true);
            GameObject.Find("Player(Clone)")?.transform.Find("EventSystem").gameObject.SetActive(false);
            GameObject.Find("Player(Clone)")?.transform.Find("CurvedUILaserPointer")?.gameObject.SetActive(false);
            GameObject.Find("LaserBeam")?.gameObject.SetActive(false);

            CCTV.transform.Find("Canvas").gameObject.SetActive(true);
            CCTV.transform.Find("EventSystem").gameObject.SetActive(true);


        }
    }

    public void ConnectBtn(Button btn)
    {

        Debug.Log("------------3.");
        // btn.onClick.AddListener(() => OnCreateRoom());
        btn.onClick.AddListener(() => OnCreateRoom());
    }

}
