using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjectionMgr : MonoBehaviour
{
    public static InjectionMgr injection = null;

    public enum STATE { Hemostasis, Disinfect, InjectArea, InjectAngle, SapSpeed, SapType };

    // 함수를 임의로 실행하기 위한 필드
    [ContextMenuItem("1. 소독", "Disinfect")]
    [ContextMenuItem("2. 주사위치", "InjectArea")]

    public STATE state = STATE.Hemostasis;
    public int[] score = { 0, 0, 0, 0 };

    public GameObject bloodLine;


    void Awake()
    {
        injection = this;
    }


    // 타이머

    void Start()
    {
        // 타이머 시작
        // 지혈 시작

    }

    // 0.지혈대

    // 1.소독
    public void Disinfect()
    {
        state = STATE.Disinfect;

        // 바이러스 활성화
        GameObject.Find("AlcoholCotton").GetComponent<AlcoholCottonMgr>().StartDisinfect();
    }

    // 2.주사 위치
    public void InjectArea()
    {
        state = STATE.InjectArea;

        // 바이러스그룹 비활성화
        GameObject.Find("VirusGroup").SetActive(false);

        // BloodLine 활성화
        bloodLine.SetActive(true);
    }

    // 3.주사 각도
    public void InjectAngle()
    {
        state = STATE.InjectAngle;

    }

    // 4.수액 종류
    public void SapType()
    {
        state = STATE.SapType;

    }

    // 5.수액 속도
    public void SapSpeed()
    {
        state = STATE.SapSpeed;

    }

    // 평가
}
