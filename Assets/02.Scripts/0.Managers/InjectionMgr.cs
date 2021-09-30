using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public TMP_Text infoTxt;
    public TMP_Text patientTxt;
    public Timer timer;
    public Image progress;

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

    [Header("물품 리스트")]
    public GameObject patient;
    public GameObject tourniquet;
    public GameObject alcoholCotton;
    public GameObject rubber;
    public GameObject[] sapList;
    public GameObject catheter = null;

    public GameObject catheterPref;
    SoundManager soundManager;



    Animator animator;

    void Awake()
    {
        injection = this;
        animator = GameObject.FindWithTag("Patient").GetComponent<Animator>();
        sapScript = GameObject.FindWithTag("Patient").GetComponent<BGB_Sap>();

        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();

        InitInjection();

    }

    void Start()
    {
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

        // 수액 초기화
        SetSap();

        // 점수 초기화
        for (int i = 0; i < scoreList.Length; i++)
        {
            // scoreList[i] = 0;
            Debug.Log($"{i} : {scoreList[i]}");
        }

        // 환자차트 UI
        patientTxt.text = $"손 위생과 물품준비가\n끝난 상황입니다.\n두드러기 환자에게\n{_sapType} 500ml를\n{_sapSpeed}cc/hr로 정맥주사\n투약해주세요.";

        // 애니메이션
        animator.SetTrigger("Idle");

        // 수액 종류 시작
        SapType();
    }

    public void CreateCatheter()
    {
        catheter = Instantiate(catheterPref);
        catheter.name = "Catheter";
    }
    #region 술기
    // 0.수액 종류
    [ContextMenu("0.수액 종류")]
    public void SapType()
    {
        state = STATE.SapType;
        progress.fillAmount = progressNum * 0;
        infoTxt.text = "트롤리 안에 있는 수액 중 알맞은 수액을\n골라 수액걸대에 걸어주세요.";

        // 타이머 시작
        timer.timerOn = true;

        // GameObject sap = sapList[_sapTypeidx].transform.Find("SapArrowPivot").gameObject;
    }

    // 1.지혈
    [ContextMenu("1.지혈")]
    public void Hemostasis()
    {
        soundManager.Sound(4);
        state = STATE.Hemostasis;
        progress.fillAmount = progressNum * 1;
        infoTxt.text = "정맥 상태가 양호한 부위보다 위쪽을 \n토니켓으로 묶어주세요.\n(토니켓을 집어 팔에 가져다대세요.)";

        // 토니켓 스냅 On
        tourniquet.GetComponent<KHG_Snap>().isDo = false;

        // 화살표
        GameObject.Find("Arrow_Sap").SetActive(false);
        ActiveArrow(tourniquet);

        // 애니메이션
        animator.SetTrigger("IdleToInjection");


    }

    // 2.소독
    [ContextMenu("2.소독")]
    public void Disinfect()
    {
        soundManager.Sound(4);
        state = STATE.Disinfect;
        progress.fillAmount = progressNum * 2;
        infoTxt.text = "소독솜으로 주사부위를 안에서 밖으로 둥글게 닦아주세요.\n(소독솜을 집어 세균을 없애주세요.)";

        // 바이러스 활성화
        GameObject.Find("AlcoholCotton").GetComponent<AlcoholCottonMgr>().StartDisinfect();

        // 화살표
        ActiveArrow(alcoholCotton);

    }

    // 3.주사 위치
    [ContextMenu("3.주사 위치")]
    public void InjectArea()
    {
        state = STATE.InjectArea;
        progress.fillAmount = progressNum * 3;
        infoTxt.text = "카테터를 집어 혈관에 주사해주세요.\n(주사한 이후에도 버튼을\n놓지 말아주세요.)";

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
        soundManager.Sound(4);
        state = STATE.InjectAngle;
        progress.fillAmount = progressNum * 4;
        infoTxt.text = "15~30º로 혈류방향을 따라 카테터를 정맥 내로 삽입해주세요.\n(각도를 정하신 뒤 카테터를 놓으면 주사됩니다.)";

    }

    // 5.주사 각도
    [ContextMenu("5.카테터 분리")]
    public void SeparateCatheter()
    {
        // state = STATE.InjectAngle;
        progress.fillAmount = progressNum * 5;
        infoTxt.text = "카테터 뒷부분을 분리해주세요.";

        // 화살표
        GameObject needlePivot = catheter.transform.Find("NeedleArrowPivot").gameObject;
        ActiveArrow(needlePivot);
    }

    // 6.고무관 연결
    [ContextMenu("6.고무관 연결")]
    public void ConnectRubber()
    {
        soundManager.Sound(4);
        // state = STATE.InjectAngle;
        progress.fillAmount = progressNum * 6;
        infoTxt.text = "고무관을 카테터 본체에 연결해주세요.";

        // 화살표
        GameObject rubberPivot = rubber.transform.Find("RubberArrowPivot").gameObject;
        ActiveArrow(rubberPivot);

    }

    // 7.수액 속도
    [ContextMenu("7.수액 속도")]
    public void SapSpeed()
    {
        soundManager.Sound(4);
        state = STATE.SapSpeed;
        progress.fillAmount = progressNum * 7;

        // 화살표
        Vector3 pos = GameObject.Find("SapSpeedArrowPivot").transform.position;
        Quaternion rot = Quaternion.Euler(0, 0, -35.31f);
        arrow.transform.position = pos;
        arrow.transform.rotation = rot;
        arrow.SetActive(true);

        infoTxt.text = "수액속도를 조절해주세요.\n(화살표 부분을 잡고\n조이스틱으로 속도를 조절해주세요.)";

        // 수액 속도 조절하는 콜라이더 On
        curruntSap.transform.Find("IVPole_Snap").gameObject.SetActive(true);

    }

    // 8.수액 속도
    [ContextMenu("8.토니켓 분리")]
    public void UntieTourniquet()
    {
        // state = STATE.InjectAngle;
        progress.fillAmount = progressNum * 8;
        infoTxt.text = "수액 속도를 조절한 후\n팔을 묶고있는 토니켓을 풀어주세요.";

        // 화살표
        ActiveArrow(tourniquet);
    }

    #endregion

    //! ANCHOR 평가
    [ContextMenu("10.평가")]
    public void GradeInjection()
    {
        soundManager.Sound(4);
        state = STATE.Grade;
        progress.fillAmount = 1;

        // 화살표 비활성화
        arrow.SetActive(false);

        // 타이머 정지
        timer.timerOn = false;
        int[] times = timer.GetTime();

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
        StartCoroutine(nameof(ShowScore));

    }

    IEnumerator ShowScore()
    {
        soundManager.SoundStop();
        float time = 1.0f;
        int score = 0;

        // 점수 총합
        foreach (int i in scoreList)
        {
            score += i;
        }
        InsertData(score);

        // 수액종류, 수액속도, 주사위치, 주사각도, 시간
        infoTxt.text = $"수액 종류 : {scoreList[0]}";
        soundManager.Sound(3);

        yield return new WaitForSeconds(time);
        infoTxt.text += $"\n수액 속도 : {scoreList[1]}";
        soundManager.Sound(3);

        yield return new WaitForSeconds(time);
        infoTxt.text += $"\n주사 위치 : {scoreList[2]}";
        soundManager.Sound(3);

        yield return new WaitForSeconds(time);
        infoTxt.text += $"\n주사 각도 : {scoreList[3]}";
        soundManager.Sound(3);

        yield return new WaitForSeconds(time);
        infoTxt.text += $"\n시간 : {scoreList[4]}";
        soundManager.Sound(3);

        yield return new WaitForSeconds(time);
        infoTxt.text += $"\n\n<b>총합 : {score}</b>";

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

    void InsertData(int score)
    {
        int[] times = timer.GetTime();
        int _score = score * 10000;
        int min = 60 - times[0];
        int sec = 60 - times[1];

        _score += min * 100;
        _score += sec;
        fb.InsertData("Injection", _score);
    }


    // 수액 랜덤값으로 설정
    void SetSap()
    {
        _sapTypeidx = Random.Range(0, type.Length);
        _sapType = type[_sapTypeidx];

        _sapSpeed = speed[Random.Range(0, speed.Length)];
        Debug.Log($"sap speed : {_sapSpeed}");
    }

    // 화살표 On
    void ActiveArrow(GameObject obj)
    {
        Vector3 pos = obj.transform.position;
        arrow.transform.position = pos + Vector3.up * 0.21f;

        arrow.transform.rotation = Quaternion.Euler(0, 0, 0);

        arrow.SetActive(true);
    }

    public void InactiveArrow()
    {
        arrow.SetActive(false);
    }
}
