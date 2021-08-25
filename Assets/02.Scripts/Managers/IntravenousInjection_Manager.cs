using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntravenousInjection_Manager : MonoBehaviour
{
    public enum STATE { Hemostasis, Disinfect, InjectArea, InjectAngle, SapSpeed, SapType }
    public STATE state = STATE.Hemostasis;
    public int[] score = { 0, 0, 0, 0 };

    // 타이머

    void Start()
    {
        // 타이머 시작
        // 지혈 시작
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
        // 소독 비활성화
        GameObject.Find("AlcoholCotton").GetComponent<AlcoholCottonMgr>().enabled = false;

        // BloodLine 활성화
    }

    // 3.주사 각도

    // 4.수액 종류

    // 5.수액 속도

    // 평가
}
