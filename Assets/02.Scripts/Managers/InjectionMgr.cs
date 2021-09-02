using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InjectionMgr : MonoBehaviour
{
    public static InjectionMgr injection = null;

    public enum STATE { Hemostasis, Disinfect, InjectArea, InjectAngle, SapSpeed, SapType, Grade };
    private float progressNum = 1.0f / 6.0f;

    public STATE state = STATE.Hemostasis;
    public int[] scoreList = { 0, 0, 0, 0, 0, 0 };

    public GameObject bloodLine;

    [Header("UI")]
    public TMP_Text infoTxt;
    public TMP_Text patientTxt;
    public Timer timer;
    public Image progress;

    // 수액
    private string[] type = { "5% DW", "A", "B", "C", "D" };
    private float[] speed = { 1.1f, 2.2f, 3.3f, 4.4f, 5.5f, 6.6f, 7.7f };
    private string _sapType;
    private float _sapSpeed;




    void Awake()
    {
        injection = this;
        infoTxt = GameObject.Find("Text - Info").GetComponent<TMP_Text>();
        patientTxt = GameObject.Find("Text - Patient").GetComponent<TMP_Text>();
        timer = GameObject.Find("Text - Timer").GetComponent<Timer>();
        progress = GameObject.Find("Progress").GetComponent<Image>();

        InitInjection();

    }

    void Start()
    {
    }

    // 초기화
    [ContextMenu("Init")]
    void InitInjection()
    {
        // 타이머 초기화
        timer.totalTime = 0.0f;

        // 수액 초기화
        SetSap();

        // 점수 초기화
        for (int i = 0; i < scoreList.Length; i++)
        {
            scoreList[i] = 0;
        }

        // 환자차트 UI
        patientTxt.text = $"손 위생과 물품준비가 끝난 상황입니다.\n두드러기 환자에게 {_sapType} 500ml를\n{_sapSpeed}의 속도로 정맥주사 투약해주세요.";



        // 지혈 시작
        Hemostasis();
    }

    // 0.지혈
    public void Hemostasis()
    {
        state = STATE.Hemostasis;
        progress.fillAmount = 0;
        infoTxt.text = "정맥 상태가 양호한 부위보다 위쪽을 \n지혈대로 묶어주세요.\n(지혈대를 집어 팔에 가져다대세요.)";

        // 타이머 시작
        timer.timerOn = true;

    }

    // 1.소독
    public void Disinfect()
    {
        state = STATE.Disinfect;
        progress.fillAmount = progressNum * 1;
        infoTxt.text = "소독솜으로 주사부위를 안에서 밖으로 둥글게 닦아주세요.\n(트리거 버튼으로 소독솜을 집어 세균을 없애주세요.)";

        // 바이러스 활성화
        GameObject.Find("AlcoholCotton").GetComponent<AlcoholCottonMgr>().StartDisinfect();
    }

    // 2.주사 위치
    public void InjectArea()
    {
        state = STATE.InjectArea;
        progress.fillAmount = progressNum * 2;
        infoTxt.text = "카테터를 집어 혈관에 주사해주세요.\n(주사한 이후에도 버튼을 놓지 말아주세요.)";

        // 바이러스그룹 비활성화
        GameObject.Find("VirusGroup").SetActive(false);

        // BloodLine 활성화
        bloodLine.SetActive(true);
    }

    // 3.주사 각도
    public void InjectAngle()
    {
        state = STATE.InjectAngle;
        progress.fillAmount = progressNum * 3;
        infoTxt.text = "15~30º로 혈류방향을 따라 카테터를 정맥 내로 삽입해주세요.\n(각도를 정하신 뒤 버튼을 놓으면 주사됩니다.)";

    }

    // 4.수액 종류
    public void SapType()
    {
        state = STATE.SapType;
        progress.fillAmount = progressNum * 4;
        infoTxt.text = "트롤리 안에 있는 수액 중 알맞은 수액을 골라 수액걸대에 걸어주세요.";

    }

    // 5.수액 속도
    public void SapSpeed()
    {
        state = STATE.SapSpeed;
        progress.fillAmount = progressNum * 5;

    }

    // 평가
    public void GradeInjection()
    {
        state = STATE.Grade;
        progress.fillAmount = 1;

        // 타이머 정지
        timer.timerOn = false;

    }


    // 수액 랜덤값으로 설정
    void SetSap()
    {
        _sapType = type[Random.Range(0, type.Length)];
        _sapSpeed = speed[Random.Range(0, speed.Length)];
    }
}
