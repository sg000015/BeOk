using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGB_Sap : MonoBehaviour
{
    public Animator anim;

    //환자 몸 메쉬
    public SkinnedMeshRenderer patientMesh;
    float patientSpeed;
    string PatientSapBag;
    string curSapBag;

    public GameObject sapSnap;

    KHG_Line line;
    float curSpeed = 0.0f;
    void Start()
    {
        //환자에게 맞는 수액 속도
        patientSpeed = InjectionMgr.injection._sapSpeed;

        //환자에게 맞는 수액 종류
        PatientSapBag = InjectionMgr.injection._sapType;

        line = GameObject.Find("Line").GetComponent<KHG_Line>();
    }

    //수액이 스냅 되었을 때 실행되는 콜백
    //! 콜백되도록 처리해야함
    public void SetCurSapBag(string sapBag)
    {
        //현재 걸려있는 수액 변수 초기화
        curSapBag = sapBag;
        Debug.Log("수액이 스냅됨");

        sapSnap.SetActive(false);
        InjectionMgr.injection.Hemostasis();
    }

    //버튼 클릭시 활성화 되는 함수.
    //Up 클릭시 +1, Down 클릭시 -1;
    public void UpdateSpeed(int num)
    {
        curSpeed += num;

        // 수액 스피드를 초과
        if (curSpeed > patientSpeed)
        {
            anim.SetBool("Trumble", true);
            return;
        }
        // 수액 스피드 미만
        else if (curSpeed < 0)
        {
            curSpeed = 0;
            return;
        }
        // 수액 스피드 적절
        else
        {
            // 수액 채워짐
            line.SetLineState(1);
            Invoke("InitBlood", 4.0f);

            // 정상인상태. 애니메이션이 작동중이라면 디폴트로 전환
            anim.SetBool("Trumble", false);

        }

        void InitBlood()
        {
            line.SetLineState(2);
        }



        // if (PatientSapBag == curSapBag)
        // {
        //     //같은 종류의 수액이라면 팔에 두드러기가 사라짐
        //     byte alphaHives = (byte)Mathf.Round(255 - (curSpeed / patientSpeed * 255));
        //     handMesh.materials[1].color = new Color32(255, 255, 255, alphaHives);
        // }
        // else
        // {
        //     //다른종류의 수액이라면 팔이 보라색으로 변함
        //     byte alphaPurple = (byte)Mathf.Round((curSpeed / patientSpeed * 255));
        //     if (alphaPurple > 50) alphaPurple = 50;

        //     //환자 팔 보라색변하는정도. 아직 조정이필요함
        //     handMesh.materials[3].color = new Color32(178, 0, 255, alphaPurple);
        //     //환자 몸 보라색변하는정도. 아직 조정이필요함
        //     patientMesh.materials[1].color = new Color32(255, 255, 255, alphaPurple);
        // }
    }

}
