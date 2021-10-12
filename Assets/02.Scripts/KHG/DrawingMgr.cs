using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DrawingMgr : MonoBehaviourPunCallbacks
{

    public static DrawingMgr drawing;
    public Transform patientArm;
    public bool isMenu = false;

    public GameObject ExitMenu;


    public GameObject virusgroup;
    public Transform syringe;
    public Transform playObject;
    public Transform alcoholcotton;
    public Transform torniquet;
    public Transform vaccum1;
    public Transform vaccum2;
    public Transform vaccumRack;
    public GameObject startPanel;

    public Transform arrow;
    public Transform arrow2;
    public UIManager_blood UImanager;
    public SoundManager soundManager;

    public int[] scoreList = { 20, 20, 20, 20, 20 };
    // public int[] vaccumList = new int[2];
    public Timer timer;

    PhotonView pv;


    public bool syringeGrab = false;


    bool finish = false;


    void Awake()
    {
        drawing = this;
        NetworkManager.instanceNW.InstantiatePlayer();
    }


    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            SetVial();

        pv = gameObject.GetPhotonView();
        StartCoroutine("ArrowActive", torniquet);
        StartCoroutine("BgmPlay");
    }





    void Update()
    {
        if (syringe.parent == null)
        {
            syringeGrab = false;
        }
        else if (syringe.parent.name == "CustomHandRight" || syringe.parent.name == "CustomHandLeft")
        {
            syringeGrab = true;
        }

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
            pv.RPC("OutToLobbyRPC", RpcTarget.AllViaServer);

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


    //토니캣 착용시
    [ContextMenu("1.지혈")]
    public void Hemostasis()
    {
        pv.RPC(nameof(HemostasisRPC), RpcTarget.AllViaServer);
    }
    [PunRPC]
    void HemostasisRPC()
    {
        virusgroup.SetActive(true);
        UImanager.UpdateUI(1);

        arrow.gameObject.SetActive(true);
        arrow.position = alcoholcotton.position + Vector3.up * 0.1f;
        StartCoroutine("ArrowActive", alcoholcotton);
    }

    //알콜솜 끝날시
    [ContextMenu("2.소독")]
    public void Disinfect()
    {
        pv.RPC(nameof(DisinfectRPC), RpcTarget.AllViaServer);

    }
    [PunRPC]
    public void DisinfectRPC()
    {
        //주사기 당기기 로직 활성화
        syringe.Find("Syringe_Back").Find("Pull_Snap").GetComponent<KHG_Snap_Draw>().AirOffStart();
        UImanager.UpdateUI(2);

        arrow.gameObject.SetActive(true);
        arrow.position = syringe.position + Vector3.up * 0.1f;
        StartCoroutine("ArrowActive", syringe);
    }

    //주사 공기 뻇을시
    [ContextMenu("3-1.주사 공기 뺴기")]
    public void SyringeAirOff()
    {
        pv.RPC(nameof(SyringeAirOffRPC), RpcTarget.AllViaServer);
    }
    [PunRPC]
    public void SyringeAirOffRPC()
    {
        //주사 커버 로직 활성화
        syringe.Find("Front").Find("Needle_Cover").GetComponent<KHG_Snap_Draw>().AirCoverStart();
        UImanager.UpdateUI(3);
    }

    //주사 안전캡 제거시
    [ContextMenu("3-2.주사기 분리")]
    public void SyringeSafeCap()
    {
        pv.RPC(nameof(SyringeSafeCapRPC), RpcTarget.AllViaServer);
    }
    [PunRPC]
    public void SyringeSafeCapRPC()
    {
        //혈액 콜라이더 활성화
        Transform needleSnap = patientArm.Find("Needle_Snap");
        needleSnap.GetComponent<BoxCollider>().enabled = true;
        needleSnap.GetComponent<MeshRenderer>().enabled = true;
        UImanager.UpdateUI(4);


        arrow.gameObject.SetActive(true);
        arrow.position = needleSnap.position + Vector3.up * 0.1f;
        StartCoroutine("ArrowActive", needleSnap);
    }


    //주사 시행 완료시
    [ContextMenu("3-3.주사기 위치,각도설정")]
    public void SyringeArea()
    {
        pv.RPC(nameof(SyringeAreaRPC), RpcTarget.AllViaServer);

    }
    [PunRPC]
    public void SyringeAreaRPC()
    {
        //주사기 스냅완료
        //주사뒷부분 당기기(주사기 잡은 상태로, 뒷부분 당길것)
        syringe.Find("Syringe_Back").Find("Pull_Snap").GetComponent<KHG_Snap_Draw>().DrawingStart();
        UImanager.UpdateUI(5);
    }


    //피 뽑기 완료 시
    [ContextMenu("4. 피 뽑기")]
    public void BloodDrawing()
    {
        pv.RPC(nameof(BloodDrawingRPC), RpcTarget.AllViaServer);

    }
    [PunRPC]
    public void BloodDrawingRPC()
    {
        //지혈대  다시 활성화
        torniquet.GetComponent<KHG_Snap_Draw>().TorniquetRestoreStart(syringe);
        UImanager.UpdateUI(6);

        //!위치 조정 필요
        arrow2.gameObject.SetActive(true);
        StartCoroutine("Arrow2Active", torniquet);
    }


    //지혈대 제거 시
    [ContextMenu("5. 지혈대 제거")]
    public void TourniquetOff()
    {
        pv.RPC(nameof(TourniquetOffRPC), RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void TourniquetOffRPC()
    {
        //알콜솜 활성화, 팔
        alcoholcotton.GetComponent<KHG_Snap_Draw>().AlcoholCottonReset();
        alcoholcotton.GetComponent<KHG_Snap_Draw>().AlcoholCottonActiveStart(syringe, patientArm);
        UImanager.UpdateUI(7);

        arrow.gameObject.SetActive(true);
        arrow.position = alcoholcotton.position + Vector3.up * 0.1f;
        StartCoroutine("ArrowActive", alcoholcotton);
    }

    //혈액 지압했을 경우
    [ContextMenu("6. 혈액 안정화")]
    public void BloodSafety()
    {
        pv.RPC(nameof(BloodSafetyRPC), RpcTarget.AllViaServer);

    }
    [PunRPC]
    public void BloodSafetyRPC()
    {
        // 주사기 분리하기 (스냅 해제, 키네마틱으로 일단 고정)
        // 진공튜브에 닿으면 스냅되고, 뒷부분 다시 활성화
        syringe.transform.Find("Front").Find("Needle_Point").GetComponent<KHG_Snap_Draw>().SyringeOffStart();
        UImanager.UpdateUI(8);

        arrow.gameObject.SetActive(true);
        arrow2.gameObject.SetActive(true);
        arrow.position = vaccum1.position + Vector3.up * 0.1f;
        arrow2.position = vaccum2.position + Vector3.up * 0.1f;
        StartCoroutine("ArrowActive", vaccum1);
        StartCoroutine("Arrow2Active", vaccum2);
    }


    bool isVaccumTube = false;

    //혈액 보관완료 한 뒤
    [ContextMenu("7. 진공튜브")]
    public void VaccumTube(Transform _transform)
    {
        pv.RPC(nameof(VaccumTubeRPC), RpcTarget.AllViaServer, _transform.name);
    }
    [PunRPC]
    public void VaccumTubeRPC(string _transformName)
    {
        GameObject.Find(_transformName).GetComponent<KHG_Snap_Draw>().ShakingStart();
        //다 담은 뒤, 주사기 다시잡으면 분리가능
        // 분리후에 흔들 것 
        if (isVaccumTube)
        {
            UImanager.UpdateUI(9);
        }
        else
        {
            isVaccumTube = true;
        }
    }

    bool isBloodShake = false;
    //혈액 흔들기
    [ContextMenu("8. 혈액 흔들기")]
    public void BloodShake(Transform _transform)
    {
        pv.RPC(nameof(BloodShakeRPC), RpcTarget.AllViaServer, _transform.name);
    }
    [PunRPC]
    public void BloodShakeRPC(string transformName)
    {
        Transform _transform = GameObject.Find(transformName).transform;
        //진공튜브 꽂는곳 활성화 하기
        _transform.GetComponent<KHG_Snap_Draw>().VialSnapStart();

        if (isBloodShake)
        {
            UImanager.UpdateUI(10);
            arrow.gameObject.SetActive(true);
            arrow.position = vaccumRack.position + Vector3.up * 0.1f;
        }
        else
        {
            arrow.gameObject.SetActive(true);
            arrow.position = vaccumRack.position + Vector3.up * 0.1f;
            isBloodShake = true;
        }
    }

    //평가
    // [ContextMenu("9. 점수 표기")]
    public void Finish()
    {
        pv.RPC(nameof(FinishRPC), RpcTarget.AllViaServer);

    }
    [PunRPC]
    public void FinishRPC()
    {
        //!점수
        if (!finish)
        {
            finish = true;
            arrow.gameObject.SetActive(false);
        }
        else if (finish)
        {
            SetErrorScore();
            soundManager.SoundStop();
            StopCoroutine("BgmPlay");
            // Timer Off
            timer.timerOn = false;
            //점수 출력
            UImanager.UpdateUI(11);
            // if (NetworkManager.instanceNW.isOculus)
            // {
            //     GameObject.Find("Player(Clone)").transform.Find("CurvedUILaserPointer").gameObject.SetActive(true);
            //     GameObject.Find("LeftHandAnchor").transform.Find("LaserBeam")?.gameObject.SetActive(true);
            //     GameObject.Find("RightHandAnchor").transform.Find("LaserBeam")?.gameObject.SetActive(true);
            // }
        }
    }



    //Tramsform 잡으면 SetActive(false)
    IEnumerator ArrowActive(Transform _tr)
    {
        WaitForSeconds ws = new WaitForSeconds(0.1f);
        while (true)
        {
            if (_tr.parent != null)
            {
                if (_tr.parent.name == "CustomHandRight" || _tr.parent.name == "CustomHandLeft")
                {
                    arrow.gameObject.SetActive(false);
                    StopCoroutine("ArrowActive");
                }
            }
            yield return ws;
        }
    }


    IEnumerator Arrow2Active(Transform _tr)
    {
        WaitForSeconds ws = new WaitForSeconds(0.1f);
        while (true)
        {
            if (_tr.parent != null)
            {
                if (_tr.parent.name == "CustomHandRight" || _tr.parent.name == "CustomHandLeft")
                {
                    arrow2.gameObject.SetActive(false);
                    StopCoroutine("Arrow2Active");
                }
            }
            yield return ws;
        }
    }

    IEnumerator BgmPlay()
    {
        float length = soundManager.audios[11].length;
        while (true)
        {
            soundManager.Sound(11);
            soundManager.Sound(11);
            soundManager.Sound(11);
            soundManager.Sound(11);
            soundManager.Sound(11);
            soundManager.Sound(11);
            yield return new WaitForSeconds(length);

        }
    }


    int random = -1;
    void SetVial()
    {
        random = Random.Range(0, 2);

        if (random == 0)
        {
            Vector3 temp = vaccum1.transform.position;
            vaccum1.transform.position = vaccum2.transform.position;
            vaccum2.transform.position = temp;
        }
    }

    void SetErrorScore()
    {
        for (int i = 0; i < scoreList.Length; i++)
        {
            if (scoreList[i] < 0) scoreList[i] = 0;
            else if (scoreList[i] > 20) scoreList[i] = 20;
        }

    }


    public void OnClickStartBtn()
    {
        pv.RPC(nameof(StartBtnRPC), RpcTarget.AllViaServer);
        GameObject.Find("LaserBeam").gameObject.SetActive(false);
        GameObject.Find("CurvedUILaserPointer").gameObject.SetActive(false);

    }

    [PunRPC]
    void StartBtnRPC()
    {
        startPanel.SetActive(false);
        torniquet.GetComponent<BoxCollider>().enabled = true;
        arrow.gameObject.SetActive(true);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            pv.RPC(nameof(EnterRoomRPC), RpcTarget.Others, random);
        }
       if (PhotonNetwork.CurrentRoom.PlayerCount > 0)
       {
           pv.RPC("StartUI", RpcTarget.AllViaServer);
       }

    }
    [PunRPC]
    void EnterRoomRPC(int _random)
    {
        if (_random == 0)
        {
            Vector3 temp = vaccum1.transform.position;
            vaccum1.transform.position = vaccum2.transform.position;
            vaccum2.transform.position = temp;
        }
    }

    public void OnClickOutToLobby()
    {
        pv.RPC(nameof(OutToLobbyRPC), RpcTarget.AllViaServer);
    }

    [PunRPC]
    void OutToLobbyRPC()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Lobby");
    }

    public GameObject inDirector;
    public GameObject StartStatePanel;

    [PunRPC]
    void StartUI()
    {
        inDirector.SetActive(true);
        StartStatePanel.SetActive(false);
    }
}
