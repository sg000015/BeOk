using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class InjectionMgr : MonoBehaviour
{
    public static InjectionMgr injection = null;
    public FirebaseManager fb;

    public enum STATE { SapType, Hemostasis, Disinfect, InjectArea, InjectAngle, SapSpeed, Grade };
    private float progressNum = 1.0f / 9.0f;

    public STATE state = STATE.Hemostasis;

    // 수액종류, 수액속도, 주사위치, 주사각도, 시간
    private int[] scoreList = { 20, 20, 20, 20, 20 };

    public GameObject bloodLine;

    // UI
    [Header("UI")]
    public RankingCtrl rankingCtrl;
    public TMP_Text infoTxt;
    public TMP_Text infoTxtPhone;
    public TMP_Text patientTxt;
    public TMP_Text patientTxtPhone;
    public Timer timer;
    public Image progress;
    public Image progressPhone;


    // Ranking UI
    [Header("Ranking UI")]
    public TMP_Text[] names;
    public TMP_Text[] scores;
    public TMP_Text[] times;

    // 수액
    private string[] type = { "Normal Saline", "5% Dextrose", "0.45% Saline" };
    private int[] speed = { 30, 35, 40, 45, 50 };
    public string _sapType;
    private int _sapTypeidx;
    public int _sapSpeed;
    public GameObject curruntSap;
    private BGB_Sap sapScript;

    // 화살표
    public GameObject arrow;
    public GameObject arrow2;
    public GameObject arrow3;


    [Header("물품 리스트")]
    public GameObject patient;
    public GameObject tourniquet;
    public GameObject alcoholCotton;
    public GameObject rubber;
    public GameObject[] sapList;
    public GameObject catheter = null;

    public GameObject catheterPref;
    public GameObject PhoneCanvas;

    public GameObject startPanel;
    SoundManager soundManager;

    PhotonView pv;

    Animator animator;

    public GoogleSheetManager sheet;



    void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // EventOculus.SetActive(true);
        }
        else
        {
            PhoneCanvas.SetActive(true);
        }
        injection = this;
        animator = GameObject.FindWithTag("Patient").GetComponent<Animator>();
        sapScript = GameObject.FindWithTag("Patient").GetComponent<BGB_Sap>();

        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();

        pv = gameObject.GetComponent<PhotonView>();

        sheet = GameObject.Find("NetworkManager").GetComponent<GoogleSheetManager>();

    }

    [ContextMenu("OnclcikStart")]
    public void OnClickStart()
    {
        //더 이상 들어오지 못하게 문닫기.
        PhotonNetwork.CurrentRoom.IsOpen = false;

        //!1013
        OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);

        int index = Random.Range(0, type.Length);
        string sapType = type[index];
        int sapSpeed = speed[Random.Range(0, speed.Length)];

        GameObject.Find("LaserBeam").gameObject.SetActive(false);
        GameObject.Find("CurvedUILaserPointer").gameObject.SetActive(false);
        pv.RPC("SetSapRPC", RpcTarget.AllViaServer, index, sapType, sapSpeed);
        InitInjection();
    }


    [PunRPC]
    void SetSapRPC(int idx, string sapType, int sapSpeed)
    {
        startPanel.SetActive(false);
        _sapTypeidx = idx;
        _sapType = sapType;
        _sapSpeed = sapSpeed;
        SapType();

        arrow.SetActive(true);
        arrow2.SetActive(true);
        arrow3.SetActive(true);


        // 환자차트 UI
        patientTxt.text = $"손 위생과 물품준비가\n끝난 상황입니다.\n두드러기 환자에게\n{_sapType} 500ml를\n{_sapSpeed}cc/hr로 정맥주사\n투약해주세요.";
        patientTxtPhone.text = $"손 위생과 물품준비가 끝난 상황입니다.\n 두드러기 환자에게 {_sapType} 500ml를\n{_sapSpeed}cc/hr로 정맥주사 투약해주세요.";
    }
    void Start()
    {
        StartCoroutine("BgmPlay");
    }

    // 초기화
    // [ContextMenu("Init")]
    void InitInjection()
    {
        // 혈관 초기화
        patient.GetComponent<SkinnedMeshRenderer>().materials[1].color = new Color(1f, 1f, 1f, 0f); //!숫자보정

        // 카테터 생성
        if (catheter != null)
        {
            Destroy(catheter);
        }
        CreateCatheter();

        // 타이머 초기화
        timer.totalTime = 0.0f;


        // 점수 초기화
        for (int i = 0; i < scoreList.Length; i++)
        {
            // scoreList[i] = 0;
            // Debug.Log($"{i} : {scoreList[i]}");
        }

        // 애니메이션
        animator.SetTrigger("Idle");

    }

    public void CreateCatheter()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            catheter = PhotonNetwork.Instantiate("Catheter_Res", new Vector3(-5.283f, 0.8f, 0.727f), Quaternion.Euler(0, 180, 0));
            catheter.transform.localScale = Vector3.one * 0.5f;
            pv.RPC(nameof(CatheterInit), RpcTarget.AllViaServer);
        }
    }

    [PunRPC]
    void CatheterInit()
    {
        GameObject.Find("Catheter_Res(Clone)").gameObject.name = "Catheter";
    }
    #region 술기
    // 0.수액 종류
    [ContextMenu("0.수액 종류")]

    public void SapType()
    {
        // pv.RPC(nameof(SapTypeRPC), RpcTarget.AllViaServer);
        state = STATE.SapType;
        progress.fillAmount = progressNum * 0;
        progressPhone.fillAmount = progressNum * 0;

        infoTxt.text = sheet.Sentence_Injection[0];
        infoTxtPhone.text = sheet.Sentence_Injection[0];
        // infoTxt.text = "트롤리 안에 있는 수액 중 알맞은 수액을\n골라 수액걸대에 걸어주세요.";
        // infoTxtPhone.text = "트롤리 안에 있는 수액 중 알맞은 수액을\n골라 수액걸대에 걸어주세요.";

        // 타이머 시작
        timer.timerOn = true;
        // GameObject sap = sapList[_sapTypeidx].transform.Find("SapArrowPivot").gameObject;
    }
    [PunRPC]
    void SapTypeRPC()
    {
        state = STATE.SapType;
        progress.fillAmount = progressNum * 0;
        progressPhone.fillAmount = progressNum * 0;

        infoTxt.text = sheet.Sentence_Injection[0];
        infoTxtPhone.text = sheet.Sentence_Injection[0];

        // 타이머 시작
        timer.timerOn = true;
    }

    // 1.지혈
    [ContextMenu("1.지혈")]
    public void Hemostasis()
    {

        pv.RPC(nameof(HemostasisRPC), RpcTarget.AllViaServer);
    }

    [PunRPC]
    void HemostasisRPC()
    {
        arrow2.SetActive(false);
        arrow3.SetActive(false);
        soundManager.Sound(4);
        state = STATE.Hemostasis;
        progress.fillAmount = progressNum * 1;
        progressPhone.fillAmount = progressNum * 1;

        infoTxt.text = sheet.Sentence_Injection[1];
        infoTxtPhone.text = sheet.Sentence_Injection[1];

        // infoTxt.text = "정맥 상태가 양호한 부위보다 위쪽을 \n토니켓으로 묶어주세요.\n(토니켓을 집어 팔에 가져다대세요.)";
        // infoTxtPhone.text = "정맥 상태가 양호한 부위보다 위쪽을 \n토니켓으로 묶어주세요.(토니켓을 집어 팔에 가져다대세요.)";


        // 토니켓 스냅 On
        tourniquet.GetComponent<KHG_Snap>().isDo = false;

        // 화살표
        GameObject.Find("Arrow_Sap")?.SetActive(false);
        ActiveArrow(tourniquet);

        // 애니메이션
        animator.SetTrigger("IdleToInjection");
    }
    // 2.소독
    [ContextMenu("2.소독")]
    public void Disinfect()
    {
        pv.RPC(nameof(DisinfectRPC), RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void DisinfectRPC()
    {
        soundManager.Sound(4);
        state = STATE.Disinfect;
        progress.fillAmount = progressNum * 2;
        progressPhone.fillAmount = progressNum * 2;

        infoTxt.text = sheet.Sentence_Injection[2];
        infoTxtPhone.text = sheet.Sentence_Injection[2];
        // infoTxt.text = "소독솜으로 주사부위를 안에서 밖으로 둥글게 닦아주세요.\n(소독솜을 집어 세균을 없애주세요.)";
        // infoTxtPhone.text = "소독솜으로 주사부위를 안에서 밖으로 둥글게 닦아주세요.\n(소독솜을 집어 세균을 없애주세요.)";

        // 바이러스 활성화
        GameObject.Find("AlcoholCotton").GetComponent<AlcoholCottonMgr>().StartDisinfect();

        // 화살표
        ActiveArrow(alcoholCotton);
    }

    // 3.주사 위치
    [ContextMenu("3.주사 위치")]
    public void InjectArea()
    {
        pv.RPC(nameof(InjectAreaRPC), RpcTarget.AllViaServer);
    }
    [PunRPC]
    public void InjectAreaRPC()
    {
        state = STATE.InjectArea;
        progress.fillAmount = progressNum * 3;
        progressPhone.fillAmount = progressNum * 3;

        infoTxt.text = sheet.Sentence_Injection[3];
        infoTxtPhone.text = sheet.Sentence_Injection[3];
        // infoTxt.text = "카테터를 집어 혈관에 주사해주세요.\n(주사한 이후에도 버튼을\n놓지 말아주세요.)";
        // infoTxtPhone.text = "카테터를 집어 혈관에 주사해주세요.\n(주사한 이후에도 버튼을\n놓지 말아주세요.)";

        // BloodLine 활성화
        bloodLine.SetActive(true);

        // 화살표
        GameObject catheterPivot = GameObject.Find("CatheterArrowPivot");
        ActiveArrow(catheterPivot);
    }

    // 4.주사 각도
    [ContextMenu("4.주사 각도")]
    public void InjectAngle()
    {
        pv.RPC(nameof(InjectAngleRPC), RpcTarget.AllViaServer);
    }
    [PunRPC]
    public void InjectAngleRPC()
    {
        soundManager.Sound(4);
        state = STATE.InjectAngle;
        progress.fillAmount = progressNum * 4;
        progressPhone.fillAmount = progressNum * 4;

        infoTxt.text = sheet.Sentence_Injection[4];
        infoTxtPhone.text = sheet.Sentence_Injection[4];
        // infoTxt.text = "15~30º로 혈류방향을 따라 카테터를 정맥 내로 삽입해주세요.\n(각도를 정하신 뒤 카테터를 놓으면 주사됩니다.)";
        // infoTxtPhone.text = "15~30º로 혈류방향을 따라 카테터를 정맥 내로 삽입해주세요.\n(각도를 정하신 뒤 카테터를 놓으면 주사됩니다.)";

    }

    // 5.주사 각도
    [ContextMenu("5.카테터 분리")]
    public void SeparateCatheter()
    {
        pv.RPC(nameof(SeparateCatheterRPC), RpcTarget.AllViaServer);
    }
    [PunRPC]
    public void SeparateCatheterRPC()
    {
        // state = STATE.InjectAngle;
        progress.fillAmount = progressNum * 5;
        progressPhone.fillAmount = progressNum * 5;

        infoTxt.text = sheet.Sentence_Injection[5];
        infoTxtPhone.text = sheet.Sentence_Injection[5];
        // infoTxt.text = "카테터 뒷부분을 분리해주세요.";
        // infoTxtPhone.text = "카테터 뒷부분을 분리해주세요.";

        // 화살표
        GameObject needlePivot = catheter.transform.Find("NeedleArrowPivot").gameObject;
        ActiveArrow(needlePivot);
    }

    // 6.고무관 연결
    [ContextMenu("6.고무관 연결")]
    public void ConnectRubber()
    {
        pv.RPC(nameof(ConnectRubberRPC), RpcTarget.AllViaServer);

    }
    [PunRPC]
    public void ConnectRubberRPC()
    {
        soundManager.Sound(4);
        // state = STATE.InjectAngle;
        progress.fillAmount = progressNum * 6;
        progressPhone.fillAmount = progressNum * 6;

        infoTxt.text = sheet.Sentence_Injection[6];
        infoTxtPhone.text = sheet.Sentence_Injection[6];
        // infoTxt.text = "고무관을 카테터 본체에 연결해주세요.";
        // infoTxtPhone.text = "고무관을 카테터 본체에 연결해주세요.";

        // 화살표
        GameObject rubberPivot = rubber.transform.Find("RubberArrowPivot").gameObject;
        ActiveArrow(rubberPivot);
    }

    // 7.수액 속도
    [ContextMenu("7.수액 속도")]
    public void SapSpeed()
    {
        pv.RPC(nameof(SapSpeedRPC), RpcTarget.AllViaServer);

    }
    [PunRPC]
    public void SapSpeedRPC()
    {
        soundManager.Sound(4);
        state = STATE.SapSpeed;
        progress.fillAmount = progressNum * 7;
        progressPhone.fillAmount = progressNum * 7;

        infoTxt.text = sheet.Sentence_Injection[7];
        infoTxtPhone.text = sheet.Sentence_Injection[7];

        // 화살표
        Vector3 pos = GameObject.Find("SapSpeedArrowPivot").transform.position;
        Quaternion rot = Quaternion.Euler(0, 0, -35.31f);
        arrow.transform.position = pos;
        arrow.transform.rotation = rot;
        arrow.SetActive(true);

        // infoTxt.text = "수액속도를 조절해주세요.\n(화살표 부분을 잡고\n조이스틱으로 속도를 조절해주세요.)";
        // infoTxtPhone.text = "수액속도를 조절해주세요.\n(화살표 부분을 잡고\n조이스틱으로 속도를 조절해주세요.)";

        // 수액 속도 조절하는 콜라이더 On
        curruntSap.transform.Find("IVPole_Snap").gameObject.SetActive(true);
    }

    // 8.수액 속도
    [ContextMenu("8.토니켓 분리")]
    public void UntieTourniquet()
    {
        pv.RPC(nameof(UntieTourniquetRPC), RpcTarget.AllViaServer);
    }
    [PunRPC]
    public void UntieTourniquetRPC()
    {
        // state = STATE.InjectAngle;
        progress.fillAmount = progressNum * 8;
        progressPhone.fillAmount = progressNum * 8;

        infoTxt.text = sheet.Sentence_Injection[8];
        infoTxtPhone.text = sheet.Sentence_Injection[8];
        // infoTxt.text = "조이스틱으로 수액 속도를 조절한 후\n팔을 묶고있는 토니켓을 풀어주세요.";
        // infoTxtPhone.text = "조이스틱으로 수액 속도를 조절한 후\n팔을 묶고있는 토니켓을 풀어주세요.";

        // 화살표
        ActiveArrow(tourniquet);
    }

    #endregion

    //! ANCHOR 평가
    [ContextMenu("9.평가")]
    public void GradeInjection()
    {
        if (PhotonNetwork.IsMasterClient)
            pv.RPC(nameof(GradeInjectionRPC), RpcTarget.AllViaServer);
    }
    [PunRPC]
    public void GradeInjectionRPC()
    {
        soundManager.Sound(4);
        state = STATE.Grade;
        progress.fillAmount = 1;
        progressPhone.fillAmount = 1;

        // 화살표 비활성화
        arrow.SetActive(false);

        // 타이머 정지
        timer.timerOn = false;
        int[] times = timer.GetTime();

        //!1013
        // infoTxtPhone.text = _sapType;
        // infoTxtPhone.text += curruntSap.name;

        // 수액종류, 수액속도, 주사위치, 주사각도, 시간
        // 0.수액종류
        string tempSapName = curruntSap.name;
        string[] tempSapNames = tempSapName.Split('_');
        string curruntSapType = tempSapNames[1];
        Debug.Log(curruntSapType);


        // 수액 종류를 틀릴 경우
        if (string.Compare(_sapType, curruntSapType, true) != 0)
        {
            Debug.Log($"{_sapType}, {curruntSapType}");
            Debug.Log("수액 종류가 다름");
            scoreList[0] = 0;
        }
        else
        {
            Debug.Log($"{_sapType}, {curruntSapType}");
            Debug.Log("수액 종류가 같음");
        }


        // 1.수액속도
        int currentSapSpeed = (int)sapScript.curSpeed;
        if (_sapSpeed != currentSapSpeed)
        {
            scoreList[1] = 0;
        }

        // 2.주사위치

        // 3.주사각도

        // 4.시간
        // 일단 만점처리
        //! TODO : 시간별 점수처리

        // UI
        StartCoroutine(ShowScore());
    }



    IEnumerator ShowScore()
    {
        soundManager.SoundStop();
        StopCoroutine("BgmPlay");
        float time = 1.0f;
        int score = 0;

        // 점수 총합
        foreach (int i in scoreList)
        {
            score += i;
        }

        infoTxt.text += $"\n{curruntSap.name}";
        infoTxtPhone.text += $"\n{curruntSap.name}";

        // 수액종류, 수액속도, 주사위치, 주사각도, 시간
        infoTxt.text = $"수액 종류 : {scoreList[0]}";
        infoTxtPhone.text = $"수액 종류 : {scoreList[0]}";
        soundManager.Sound(3);

        yield return new WaitForSeconds(time);
        infoTxt.text += $"\n수액 속도 : {scoreList[1]}";
        infoTxtPhone.text += $"\n수액 속도 : {scoreList[1]}";
        soundManager.Sound(3);

        yield return new WaitForSeconds(time);
        infoTxt.text += $"\n주사 위치 : {scoreList[2]}";
        infoTxtPhone.text += $"\n주사 위치 : {scoreList[2]}";
        soundManager.Sound(3);

        yield return new WaitForSeconds(time);
        infoTxt.text += $"\n주사 각도 : {scoreList[3]}";
        infoTxtPhone.text += $"\n주사 각도 : {scoreList[3]}";
        soundManager.Sound(3);

        yield return new WaitForSeconds(time);
        infoTxt.text += $"\n시간 : {scoreList[4]}";
        infoTxtPhone.text += $"\n시간 : {scoreList[4]}";
        soundManager.Sound(3);

        yield return new WaitForSeconds(time);
        infoTxt.text += $"\n\n<b>총합 : {score}</b>";
        infoTxtPhone.text += $"\n\n<b>총합 : {score}</b>";

        // Background Sound
        // 50점 초과 && 수액 종류 맞음
        if (score > 50 && scoreList[0] != 0)
        {
            // 4초
            soundManager.Sound(6);
            soundManager.Sound(6);
            soundManager.Sound(6);
            soundManager.Sound(6);

            // Patient Sound
            if (score >= 70)
            {
                // 환자 anim
                sapScript.WrongSapType(false);

                StartCoroutine(nameof(PlayPatientSound), 4);
            }
            else
            {
                StartCoroutine(nameof(PlayPatientSound), 5);
            }
        }
        else
        {
            // 2초
            soundManager.Sound(5);
            soundManager.Sound(5);
            soundManager.Sound(5);
            soundManager.Sound(5);

            // 환자 색 anim
            sapScript.WrongSapType(true);

            // Patient Sound
            StartCoroutine(nameof(PlayPatientSound), 3);
        }

        yield return new WaitForSeconds(4);
        // 랭킹 UI
        Ranking(score);
    }

    IEnumerator PlayPatientSound(int num)
    {
        if (num == 3)
        {
            yield return new WaitForSeconds(2f);
        }
        else
        {
            yield return new WaitForSeconds(4f);
        }
        soundManager.PlayPatientSound(num);
    }




    /*
        1. 랭킹 불러오기
        2. 랭킹 안에 들었는지 확인
            a. 랭킹 안에 듦 : 키보드 > enter > 데이터 삽입
        3. 랭킹 불러오기 > UI 변경 
        4. 랭킹 UI 활성화
    */

    private int myScore = 0;
    void Ranking(int score)
    {
        // 랭킹 불러오기
        fb.LoadAllData("Injection");

        rankingCtrl.ChangeCanvas();

        StartCoroutine(nameof(CheckRanking), score);
    }

    IEnumerator CheckRanking(int score)
    {
        yield return new WaitUntil(() => fb.isLoad);
        fb.isLoad = false;

        // 랭킹 안에 들었는지 확인
        myScore = ConvertScore(score);
        Debug.Log($"lastRankerScore : {fb.lastRankerScore}");
        Debug.Log($"myScore : {myScore}");
        if (fb.lastRankerScore <= myScore)
        {
            // 랭킹 안에 들었으면 키보드 UI on
            rankingCtrl.ActiveNickname();
        }
        else
        {
            // 랭킹 안에 들지 못했으면
            StartCoroutine(nameof(ActiveRanking));
            // ActiveRanking();
        }

    }

    IEnumerator ActiveRanking()
    {
        // 랭킹 불러오기
        fb.LoadAllData("Injection");

        yield return new WaitUntil(() => fb.isLoad);
        fb.isLoad = false;

        // UI 변경
        int _rankNum = fb.rankNum;
        for (int i = 0; i < _rankNum; i++)
        {
            names[i].text = fb.rankersName[i];
            scores[i].text = $"{fb.rankersScore[i]}";
            times[i].text = $"{fb.rankersMin[i]}:{fb.rankersSec[i]}";

            Debug.Log($"____{names[i].text} / {scores[i].text} / {times[i].text}");
        }
        Debug.Log("텍스트 변경 완료");

        // UI 활성화
        rankingCtrl.ActiveRankingUI();

    }

    int ConvertScore(int score)
    {
        int[] times = timer.GetTime();
        int _score = score * 10000;
        int min = 60 - times[0];
        int sec = 60 - times[1];

        _score += min * 100;
        _score += sec;

        return _score;
    }

    public void InsertData(string nickname)
    {
        Debug.Log("MGR - InsertData");
        fb.InsertData("Injection", nickname, myScore);
        // ActiveRanking();
        StartCoroutine(nameof(ActiveRanking));
    }


    public void MinusAreaScore()
    {
        if (scoreList[2] == 0) return;

        scoreList[2] -= 5;
    }

    public void MinusAngleScore()
    {
        if (scoreList[3] == 0) return;

        scoreList[3] -= 5;
    }



    // 화살표 On
    void ActiveArrow(GameObject obj)
    {
        Vector3 pos = obj.transform.position;
        arrow.transform.position = pos + Vector3.up * 0.21f;

        arrow.transform.rotation = Quaternion.Euler(0, 0, 0);

        arrow.SetActive(true);

        if (PhotonNetwork.IsMasterClient)
        {
            pv.RPC(nameof(ActiveArrowRPC), RpcTarget.Others, obj.name);
        }
    }

    [PunRPC]
    void ActiveArrowRPC(string name)
    {
        GameObject obj = GameObject.Find(name).gameObject;

        Vector3 pos = obj.transform.position;
        arrow.transform.position = pos + Vector3.up * 0.21f;

        arrow.transform.rotation = Quaternion.Euler(0, 0, 0);

        arrow.SetActive(true);
    }

    public void InactiveArrow()
    {
        arrow.SetActive(false);
    }


    IEnumerator BgmPlay()
    {
        float length = soundManager.audios[9].length;
        while (true)
        {
            Debug.Log("Sound(9)");
            soundManager.Sound(9);
            soundManager.Sound(9);
            soundManager.Sound(9);
            soundManager.Sound(9);
            soundManager.Sound(9);
            yield return new WaitForSeconds(length);

        }
    }
}
