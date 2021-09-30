using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHG_Snap_Draw : MonoBehaviour
{

    public enum ObjectType
    {
        None,
        AlcoholCotton,
        Pull,
        NeedleCover,

        Needle,
        Tourniquet,
        IVPole,
        Sap,
        Rubber,
    }



    private Material mat;
    private bool isDo = false;

    public ObjectType _objectType;
    private SoundManager soundManager;


    //함수 실행 여부
    bool[] functionState = new bool[10];

    int airCount;
    bool airCheck;
    Transform syringe;
    Transform front;
    Transform back;
    Transform point;
    Transform tempTr;

    Vector3 rot;
    Vector3 reset;
    Vector3 pos;


    void OnTriggerEnter(Collider coll)
    {
        Debug.Log(coll.name);

        if (_objectType == ObjectType.AlcoholCotton)
        {
            if (coll.name == "VirusBody")
            {
                coll.GetComponentInParent<VirusMgr>().AlcoholCottonEnter();
                Debug.Log("check");

            }
        }


        else if (_objectType == ObjectType.Tourniquet)
        {
            if (!isDo && coll.name == "Tor_Snap")
            {
                gameObject.GetComponent<BoxCollider>().enabled = false;
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<KHG_Grabble>().grabByState = KHG_Grabble.GrabByState.None;

                transform.SetParent(coll.gameObject.transform.parent);

                transform.localPosition = new Vector3(-3.04f, -2.507f, 0.322f);
                transform.localEulerAngles = new Vector3(-90f, 0, -10f);


                // mat = DrawingMgr.drawing.patient.GetComponent<SkinnedMeshRenderer>().materials[1];
                // StartCoroutine("SetBloodLineAlpha");

                // 다음 단계 완료 : 소독
                DrawingMgr.drawing.Hemostasis();
                soundManager.Sound(4);

                isDo = true;
            }
        }
        else if (_objectType == ObjectType.Needle)
        {
            if (!isDo && coll.name == "Needle_Snap")
            {
                Debug.Log("DegreeX: " + transform.eulerAngles.x);
                Debug.Log("DegreeY: " + transform.eulerAngles.y);
                Debug.Log("DegreeZ: " + transform.eulerAngles.z);
                //!각도체크
                if (true)
                {
                    syringe = transform.parent.parent;
                    pos = syringe.position;
                    rot = syringe.eulerAngles;
                    functionState[2] = true;
                    DrawingMgr.drawing.SyringeArea();
                    soundManager.Sound(4);
                    isDo = true;


                }
            }
        }

    }

    void Start()
    {

        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();

        Debug.Log(this.name);
        // if (_objectType == ObjectType.Pull)
        // {
        //     syringeBack = GameObject.Find("Syringe_Back").transform;
        //     tag = "Untagged";
        // }
    }





    public void AirOffStart()
    {
        if (_objectType == ObjectType.Pull)
        {
            tag = "GrabObject";
            airCount = 0;
            airCheck = false;
            syringe = transform.parent.parent;
            back = transform.parent;
            functionState[0] = true;

            Debug.Log("First" + Vector3.Distance(back.position, transform.position));
        }
    }





    public void AirCoverStart()
    {
        if (_objectType == ObjectType.NeedleCover)
        {
            tag = "GrabObject";
            front = transform.parent;
            point = transform.GetChild(0);
            syringe = front.parent;

            rot = new Vector3(0, -90f, -90f);
            reset = new Vector3(0, 0.372f, -4.5f);
            functionState[1] = true;
        }
    }

    public void DrawingStart()
    {
        if (_objectType == ObjectType.Pull)
        {
            tag = "GrabObject";
            // airCount = 0;
            // airCheck = false;
            syringe = transform.parent.parent;
            back = transform.parent;
            minusNum = 50;
            functionState[3] = true;
        }
    }




    void LateUpdate()
    {

        if (functionState[0]) AirOff();
        if (functionState[1]) AirCover();
        if (functionState[2]) SyringeSnap();
        if (functionState[3]) Drawing();

    }


    void AirOff()
    {
        if (_objectType == ObjectType.Pull)
        {
            if (transform.parent == null)
            {
                Debug.Log("null");
                transform.GetComponent<Rigidbody>().isKinematic = true;
                transform.GetComponent<Rigidbody>().useGravity = false;
                transform.parent = back;


                // back.localPosition = Vector3.forward * -1.6f;
                // back.localEulerAngles = Vector3.zero;
                transform.localPosition = new Vector3(0, 0.363f, 4f);
                transform.localEulerAngles = Vector3.zero;
                airCheck = false;


                if (airCount > 2)
                {

                    back.localPosition = Vector3.forward * -1.6f;
                    back.localEulerAngles = Vector3.zero;
                    tag = "Untagged";
                    DrawingMgr.drawing.SyringeAirOff();
                    soundManager.Sound(4);
                    functionState[0] = false;
                    Destroy(this.gameObject);

                }

            }
            else if (syringe.parent != null && (transform.parent.name == "CustomHandRight" || transform.name == "CustomHandLeft"))
            {

                tempTr = transform.parent;
                transform.parent = back;

                dis = Vector3.Distance(back.position, transform.position);


                if (true)
                {
                    back.localPosition = Vector3.forward * (-1.6f + 2 * (-0.2f + Mathf.Abs(dis)));
                }

                if (!airCheck && dis > 0.4f)
                {
                    airCheck = true;
                    airCount++;
                    soundManager.Sound(0);
                }
                else if (airCheck && dis < 0.3f)
                {
                    airCheck = false;
                }
                transform.parent = tempTr;




            }
        }

    }


    void AirCover()
    {

        if (_objectType == ObjectType.NeedleCover)
        {


            point.parent = front;
            if (point.localPosition.z > -4.0f) { point.localPosition = reset; Debug.Log("reset"); }
            else { point.localPosition = new Vector3(0, 0.372f, point.localPosition.z); }
            point.localEulerAngles = rot;




            //Vector3 pos = point.position;
            transform.position = point.position;
            transform.rotation = point.rotation;

            if (transform.parent != null)
            {
                point.parent = transform;
            }

            float dis = Vector3.Distance(transform.position, front.position);
            if (syringe.parent != null && transform.parent != null && dis > 0.3f)
            {
                DrawingMgr.drawing.SyringeSafeCap();
                soundManager.Sound(4);
                tag = "Untagged";
                functionState[1] = false;

                Destroy(this.gameObject);
            }

        }

    }

    void SyringeSnap()
    {
        syringe.position = pos;
        syringe.eulerAngles = rot;
    }




    float dis;
    float lastDis;


    int minusNum = 0;
    int minusNum2 = 0;

    void Drawing()
    {
        if (minusNum2 > 0) { minusNum2--; }
        if (minusNum > 0) { minusNum--; }

        if (_objectType == ObjectType.Pull)
        {
            //!주사기 놓을시 감점(딜레이 주의) : "주사기를 잡아주세요"
            if (syringe.parent == null)
            {
                transform.GetComponent<Rigidbody>().isKinematic = true;
                transform.GetComponent<Rigidbody>().useGravity = false;
                transform.parent = back;

                // back.localPosition = Vector3.forward * -1.6f;
                // back.localEulerAngles = Vector3.zero;

                dis = Vector3.Distance(back.position, transform.position);
                transform.localPosition = new Vector3(0, 0.363f, 4f);
                transform.localEulerAngles = Vector3.zero;

                if (minusNum < 1)
                {
                    minusNum = 300;
                    soundManager.Sound(2);
                    soundManager.Sound(2);
                    soundManager.Sound(2);
                    Debug.Log("주사기 들기 감점");
                }
            }
            else if (syringe.parent != null && (transform.parent.name == "CustomHandRight" || transform.name == "CustomHandLeft"))
            {

                transform.parent = tempTr;
                dis = Vector3.Distance(back.position, transform.position);

                //! 이동거리 차이에 따라 감점(속도)
                if ((dis - lastDis) * 1000 > 3)
                {
                    Debug.Log("감점");
                    if (minusNum2 < 1)
                    {
                        minusNum2 = 100;
                        soundManager.Sound(2);
                        soundManager.Sound(2);
                        soundManager.Sound(2);
                        Debug.Log("주사기 속도 감점");
                    }
                }
                lastDis = dis;

                if (true)
                {
                    back.localPosition = Vector3.forward * (-1.6f + 2 * (-0.2f + Mathf.Abs(dis)));
                    //피 채우기(dis를 이용)
                }

                if (dis > 0.5f)
                {
                    soundManager.Sound(4);
                    functionState[3] = false;
                    functionState[2] = false;
                    DrawingMgr.drawing.BloodDrawing();
                }

                transform.parent = tempTr;




            }
        }

    }
}
