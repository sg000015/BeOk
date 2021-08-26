using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntravenousInjection_Manager : MonoBehaviour
{
    public enum STATE { Hemostasis, Disinfect, InjectArea, InjectAngle, SapSpeed, SapType };

    // 함수를 임의로 실행하기 위한 필드
    [ContextMenuItem("1. 소독", "Disinfect")]
    [ContextMenuItem("2. 주사위치", "InjectArea")]

    public STATE state = STATE.Hemostasis;
    public int[] score = { 0, 0, 0, 0 };

    public GameObject bloodLine;


    // 타이머

    void Start()
    {
        // 타이머 시작
        // 지혈 시작

        //!
        Disinfect();
    }

    void Update()
    {

    }

    // 0.지혈대

    // 1.소독
    public void Disinfect()
    {
        // 지혈 비활성화
        //todo : 지혈대 콜라이더 끄는 등...

        // 바이러스 활성화
        GameObject.Find("AlcoholCotton").GetComponent<AlcoholCottonMgr>().StartDisinfect();
    }

    // 2.주사 위치
    public void InjectArea()
    {
        // 소독 스크립트 제거
        Destroy(GameObject.Find("AlcoholCotton").GetComponent<AlcoholCottonMgr>());
        // 바이러스 제거
        Destroy(GameObject.Find("VirusGroup"));

        // BloodLine 활성화
        bloodLine.SetActive(true);
    }

    // 3.주사 각도

    // 4.수액 종류

    // 5.수액 속도

    // 평가
}
