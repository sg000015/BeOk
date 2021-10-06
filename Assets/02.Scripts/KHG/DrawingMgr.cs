using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingMgr : MonoBehaviour
{

    public static DrawingMgr drawing;
    public Transform patientArm;

    public GameObject virusgroup;
    public Transform syringe;
    public Transform playObject;
    public Transform alcoholcotton;
    public Transform arrow;
    public UIManager_blood UImanager;

    public int[] scoreList = { 20, 20, 20, 20, 20 };


    public bool syringeGrab = false;

    bool finish = false;



    void Awake()
    {
        drawing = this;
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
    }

    //토니캣 착용시
    [ContextMenu("1.지혈")]
    public void Hemostasis()
    {
        virusgroup.SetActive(true);
        UImanager.UpdateUI(11);

    }

    //알콜솜 끝날시
    [ContextMenu("2.소독")]
    public void Disinfect()
    {
        //주사기 당기기 로직 활성화
        alcoholcotton.GetComponent<KHG_Snap_Draw>().AlcoholCottonReset();
        syringe.Find("Syringe_Back").Find("Pull_Snap").GetComponent<KHG_Snap_Draw>().AirOffStart();
    }

    //주사 공기 뻇을시
    [ContextMenu("3-1.주사 공기 뺴기")]
    public void SyringeAirOff()
    {
        //주사 커버 로직 활성화
        syringe.Find("Front").Find("Needle_Cover").GetComponent<KHG_Snap_Draw>().AirCoverStart();
    }

    //주사 안전캡 제거시
    [ContextMenu("3-2.주사기 분리")]
    public void SyringeSafeCap()
    {
        //혈액 콜라이더 활성화
        Transform needleSnap = patientArm.Find("Needle_Snap");
        needleSnap.GetComponent<BoxCollider>().enabled = true;
        needleSnap.GetComponent<MeshRenderer>().enabled = true;

    }

    //주사 시행 완료시
    [ContextMenu("3-3.주사기 위치,각도설정")]
    public void SyringeArea()
    {
        //주사기 스냅완료
        //주사뒷부분 당기기(주사기 잡은 상태로, 뒷부분 당길것)
        syringe.Find("Syringe_Back").Find("Pull_Snap").GetComponent<KHG_Snap_Draw>().DrawingStart();
    }


    //피 뽑기 완료 시
    [ContextMenu("4. 피 뽑기")]
    public void BloodDrawing()
    {
        //지혈대  다시 활성화
        patientArm.Find("Tourniquet").GetComponent<KHG_Snap_Draw>().TorniquetRestoreStart(syringe);
    }

    //지혈대 제거 시
    [ContextMenu("5. 지혈대 제거")]
    public void TourniquetOff()
    {
        //알콜솜 활성화, 팔
        alcoholcotton.GetComponent<KHG_Snap_Draw>().AlcoholCottonActiveStart(syringe, patientArm);
    }

    //혈액 지압했을 경우
    [ContextMenu("6. 혈액 안정화")]
    public void BloodSafety()
    {
        // 주사기 분리하기 (스냅 해제, 키네마틱으로 일단 고정)
        // 진공튜브에 닿으면 스냅되고, 뒷부분 다시 활성화
        syringe.transform.Find("Front").Find("Needle_Point").GetComponent<KHG_Snap_Draw>().SyringeOffStart();




    }


    //혈액 보관완료 한 뒤
    [ContextMenu("7. 진공튜브")]
    public void VaccumTube(Transform _transform)
    {
        //다 담은 뒤, 주사기 다시잡으면 분리가능
        // 분리후에 흔들 것 
        _transform.GetComponent<KHG_Snap_Draw>().ShakingStart();

    }

    //혈액 흔들기
    [ContextMenu("8. 혈액 흔들기")]
    public void BloodShake(Transform _transform)
    {
        //!진공튜브 꽂는곳 활성화 하기
        _transform.GetComponent<KHG_Snap_Draw>().VialSnapStart();

    }
    //평가
    [ContextMenu("9. 점수 표기")]
    public void Finish()
    {
        //!점수
        if (!finish)
        {
            finish = true;
        }
        else if (finish)
        {
            //점수 출력
        }
    }


}
