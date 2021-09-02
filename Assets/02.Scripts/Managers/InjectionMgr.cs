using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InjectionMgr : MonoBehaviour
{
    public static InjectionMgr injection = null;

    public enum STATE { Hemostasis, Disinfect, InjectArea, InjectAngle, SapSpeed, SapType };

    public STATE state = STATE.Hemostasis;
    public int[] score = { 0, 0, 0, 0 };

    public GameObject bloodLine;

    [Header("UI")]
    public TMP_Text infoTxt;




    void Awake()
    {
        injection = this;
        infoTxt = GameObject.Find("Text - Info").GetComponent<TMP_Text>();

    }


    // 타이머

    void Start()
    {
        // 타이머 시작

        // 지혈 시작
        infoTxt.text = "정맥 상태가 양호한 부위보다 위쪽을 \n지혈대로 묶어주세요.\n(지혈대를 집어 팔에 가져다대세요.)";

    }

    // 0.지혈대

    // 1.소독
    public void Disinfect()
    {
        state = STATE.Disinfect;
        infoTxt.text = "소독솜으로 주사부위를 안에서 밖으로 둥글게 닦아주세요.\n(트리거 버튼으로 소독솜을 집어 세균을 없애주세요.)";

        // 바이러스 활성화
        GameObject.Find("AlcoholCotton").GetComponent<AlcoholCottonMgr>().StartDisinfect();
    }

    // 2.주사 위치
    public void InjectArea()
    {
        state = STATE.InjectArea;
        infoTxt.text = "카테터를 집어 혈관에 주사해주세요.\n(주사한 이후에도 버튼을 놓지 말아주세요.)";

        // 바이러스그룹 비활성화
        GameObject.Find("VirusGroup").SetActive(false);

        // BloodLine 활성화
        bloodLine.SetActive(true);
    }

    // 3.주사 각도
    public void InjectAngle()
    {
        infoTxt.text = "15~30º로 혈류방향을 따라 카테터를 정맥 내로 삽입해주세요.\n(각도를 정하신 뒤 버튼을 놓으면 주사됩니다.)";
        state = STATE.InjectAngle;

    }

    // 4.수액 종류
    public void SapType()
    {
        infoTxt.text = "트롤리 안에 있는 수액 중 알맞은 수액을 골라 수액걸대에 걸어주세요.";
        state = STATE.SapType;

    }

    // 5.수액 속도
    public void SapSpeed()
    {
        state = STATE.SapSpeed;

    }

    // 평가
}
