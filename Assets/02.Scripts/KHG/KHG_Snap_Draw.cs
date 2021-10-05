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
        Syringe,
        Shake,
        VaccumTube,

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

    Transform syringe;
    Transform front;
    Transform back;
    Transform point;
    Transform tempTr;
    public Transform vaccum;

    Vector3 rot;
    Vector3 reset;
    Vector3 backReset;
    Vector3 pos;


    float dis;
    float lastDis;

    int minusNum = 0;
    int minusNum2 = 0;
    int airCount = 0;

    bool airCheck;
    bool isFirst = false;


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

            else if (functionState[5])
            {
                if (coll.name == "Needle_Point")
                {
                    //! 팔에 알콜솜 스냅될것
                    soundManager.Sound(4);
                    DrawingMgr.drawing.BloodSafety();
                    isDo = true;
                }

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

                transform.SetParent(coll.transform.parent);

                transform.localPosition = new Vector3(0.1123f, -0.339f, -0.1777f);
                transform.localEulerAngles = new Vector3(43.6f, 234.04f, 40.4f);
                transform.localScale = new Vector3(0.8174f, -0.8f, -1.09f);


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
                Vector3 needleAngle = transform.eulerAngles;
                Debug.Log("DegreeX: " + needleAngle.x);
                Debug.Log("DegreeY: " + needleAngle.y);
                // Debug.Log("DegreeZ: " + needleAngle.z);

                //!각도체크 감점
                if (needleAngle.x > 350f || needleAngle.x < 320 || needleAngle.y < 160 || needleAngle.y > 200)
                {
                    Debug.Log("감점요인\nDegree X : 10~40도 (350 ~ 310 사이)\nDegree Y : -20~20도 (160 ~ 200 사이)");
                }

                syringe = transform.parent.parent;
                syringe.GetComponent<Rigidbody>().isKinematic = true;
                // syringe.tag = "Untagged";
                pos = syringe.position;
                rot = syringe.eulerAngles;
                functionState[2] = true;
                DrawingMgr.drawing.SyringeArea();
                soundManager.Sound(4);
                isDo = true;


            }
            else if (isDo && coll.name == "Vaccum_Snap")
            {
                if (functionState[6])
                {
                    //!주사기 스냅 되었을시
                    vaccum = coll.transform;
                    front = transform.parent.parent.Find("Blood");
                    back = coll.transform.parent.Find("Blood");
                    soundManager.Sound(4);
                    minusNum = 0;
                    minusNum2 = 0;
                    functionState[6] = false;
                    functionState[7] = true;

                }
            }
        }
        else if (_objectType == ObjectType.Shake)
        {
            if (coll.name == "Plane")
            {
                airCount++;
                soundManager.Sound(3);
                soundManager.Sound(3);
                soundManager.Sound(3);
                Debug.Log("Shakeing Check = " + airCount);
                if (airCount > 8)
                {
                    soundManager.Sound(4);
                    DrawingMgr.drawing.BloodShake();
                    Debug.Log("Shaking Finish");
                    GetComponent<BoxCollider>().enabled = false;
                }
            }
        }
        else if (!isDo && _objectType == ObjectType.VaccumTube)
        {
            if (functionState[8] && coll.name == "Vial_Snap")
            {
                // transform.position = 
                // transform.rotation = 
                transform.parent = null;
                tag = "Untagged";
                GetComponent<Rigidbody>().isKinematic = true;
                transform.position = new Vector3(-0.365f, 0.8f, -0.53f);
                transform.eulerAngles = Vector3.zero;

                isDo = true;
                soundManager.Sound(4);
                functionState[8] = false;
                DrawingMgr.drawing.Finish();
            }
        }

    }

    [ContextMenu("Degree Check")]
    void DegreeCheck()
    {
        Vector3 needleAngle = transform.eulerAngles;
        Debug.Log(needleAngle.x);
        Debug.Log(needleAngle.y);
    }



    void Start()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }

    void LateUpdate()
    {
        if (functionState[0]) AirOff();
        if (functionState[1]) AirCover();
        if (functionState[2]) SyringeSnap();
        if (functionState[3]) Drawing();
        if (functionState[4]) TorniquetRestore();
        if (functionState[5]) AlcoholCottonActive();
        if (functionState[7]) VaccumInsert();


    }


    #region start

    public void AlcoholCottonReset()
    {

        if (_objectType == ObjectType.AlcoholCotton)
        {
            transform.parent = null;
            transform.position = new Vector3(0.4731f, 0.69f, -1.192f);
            transform.eulerAngles = Vector3.right * -84.77f;
            GetComponent<Rigidbody>().isKinematic = false;

        }
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

            rot = new Vector3(90, -90f, 0);
            reset = new Vector3(0, 0.363f, 4f);
            backReset = new Vector3(0.383f, 1.31f, 0);

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
            backReset = new Vector3(0.383f, 1.31f, 0);
            functionState[1] = true;
        }
    }

    public void DrawingStart()
    {
        if (_objectType == ObjectType.Pull)
        {
            tag = "GrabObject";
            syringe = transform.parent.parent;
            back = transform.parent;
            minusNum = 100;
            rot = new Vector3(90, -90f, 0);
            reset = new Vector3(0, 0.363f, 4f);
            backReset = new Vector3(0.383f, 1.31f, 0);
            isFirst = true;
            functionState[3] = true;
        }
    }

    public void TorniquetRestoreStart(Transform _syringe)
    {
        if (_objectType == ObjectType.Tourniquet)
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            gameObject.GetComponent<KHG_Grabble>().grabByState = KHG_Grabble.GrabByState.All;

            syringe = _syringe;
            minusNum = 100;
            functionState[4] = true;
        }

    }

    public void AlcoholCottonActiveStart(Transform _syringe, Transform _arm)
    {

        syringe = _syringe;
        tempTr = _arm;
        minusNum = 100;
        functionState[5] = true;
    }

    public void SyringeOffStart()
    {
        syringe = transform.parent.parent;
        syringe.GetComponent<Rigidbody>().isKinematic = true;
        syringe.tag = "GrabObject";
        rot = Vector3.right * 180.0f;
        pos = Vector3.up * 10f;
        minusNum = 0;
        minusNum2 = 0;

        functionState[2] = false;
        functionState[6] = true;

        //!테스트
        // functionState[7] = true;
    }

    public void ShakingStart()
    {
        tag = "GrabObject";
        GetComponent<KHG_Grabble>().grabByState = KHG_Grabble.GrabByState.All;
        transform.Find("Shake_Snap").GetComponent<BoxCollider>().enabled = true;

    }

    public void VialSnapStart()
    {
        functionState[8] = true;
    }

    #endregion



    #region Actions(LateUpdate)

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


                back.localPosition = backReset;
                back.localEulerAngles = rot;
                transform.localPosition = reset;
                transform.localEulerAngles = Vector3.zero;
                airCheck = false;


                if (airCount > 2)
                {

                    back.localPosition = backReset;
                    back.localEulerAngles = rot;
                    tag = "Untagged";
                    DrawingMgr.drawing.SyringeAirOff();
                    soundManager.Sound(4);
                    functionState[0] = false;

                }

            }
            else if (syringe.parent != null && (transform.parent.name == "CustomHandRight" || transform.parent.name == "CustomHandLeft"))
            {
                Debug.Log(transform.parent.name);

                tempTr = transform.parent;
                transform.parent = back;

                dis = Vector3.Distance(back.position, transform.position);

                back.localPosition = backReset - Vector3.up * 8 * (-0.1f + Mathf.Abs(dis));

                if (!airCheck && dis > 0.3f)
                {
                    airCheck = true;
                    airCount++;
                    soundManager.Sound(0);
                    soundManager.Sound(0);
                    soundManager.Sound(0);
                }
                else if (airCheck && dis < 0.2f)
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
            if (syringe.parent != null && transform.parent != null && dis > 0.15f)
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

    void Drawing()
    {

        if (_objectType == ObjectType.Pull)
        {
            minusNum2--;
            if (syringe.parent == null)
            {
                minusNum--;
                //!주사기 놓을시 감점(딜레이 주의) : "주사기를 잡아주세요"
                if (minusNum < 1)
                {
                    minusNum = 300;
                    soundManager.Sound(2);
                    soundManager.Sound(2);
                    soundManager.Sound(2);
                    soundManager.Sound(10);
                    Debug.Log("주사기 들기 감점");
                }

            }
            else
            {
                minusNum = 100;
            }

            if (transform.parent == null)
            {
                isFirst = true;
                transform.GetComponent<Rigidbody>().isKinematic = true;
                transform.GetComponent<Rigidbody>().useGravity = false;
                transform.parent = back;

                back.localPosition = backReset;
                back.localEulerAngles = rot;

                dis = Vector3.Distance(back.position, transform.position);
                transform.localPosition = reset;
                transform.localEulerAngles = Vector3.zero;

            }
            else if (syringe.parent != null && (transform.parent.name == "CustomHandRight" || transform.parent.name == "CustomHandLeft"))
            {


                tempTr = transform.parent;

                dis = Vector3.Magnitude(back.position - transform.position);

                // Debug.Log("First dis" + dis);
                Debug.Log((dis - lastDis) * 1000);

                if ((dis - lastDis) * 1000 > 5)
                {
                    if (!isFirst)
                        if (minusNum2 < 1)
                        {
                            minusNum2 = 20;
                            soundManager.Sound(1);
                            soundManager.Sound(1);
                            soundManager.Sound(1);
                            //! 환자 Sound, 감점
                            soundManager.Sound(9);
                            Debug.Log("주사기 속도 감점");
                        }
                }
                isFirst = false;
                lastDis = dis;

                float fixdis = Mathf.Abs(dis);
                Debug.Log(fixdis);
                back.localPosition = backReset - Vector3.up * 15 * (-0.1f + fixdis);

                Transform blood = syringe.Find("Blood");
                blood.localPosition = Vector3.up * (1.8f + -7.3f * (fixdis - 0.1f));
                blood.localScale = new Vector3(0.5f, 7.3f * (fixdis - 0.1f), 0.5f);

                if (dis > 0.25f)
                {
                    soundManager.Sound(4);
                    functionState[3] = false;
                    tag = "Untagged";
                    DrawingMgr.drawing.BloodDrawing();

                }

                transform.parent = tempTr;




            }
        }

    }

    void TorniquetRestore()
    {
        if (_objectType == ObjectType.Tourniquet)
        {

            if (syringe.parent == null)
            {
                minusNum--;
                //!주사기 놓을시 감점(딜레이 주의) : "주사기를 잡아주세요"
                if (minusNum < 1)
                {
                    minusNum = 300;
                    soundManager.Sound(2);
                    soundManager.Sound(2);
                    soundManager.Sound(2);
                    soundManager.Sound(10);
                    Debug.Log("주사기 들기 감점");
                }

            }
            if (transform.parent == null)
            {
                soundManager.Sound(4);
                DrawingMgr.drawing.TourniquetOff();
                functionState[4] = false;
            }
        }
    }

    void AlcoholCottonActive()
    {
        if (_objectType == ObjectType.AlcoholCotton)
        {

            if (syringe.parent == null)
            {
                minusNum--;
                //!주사기 놓을시 감점(딜레이 주의) : "주사기를 잡아주세요"
                if (minusNum < 1)
                {
                    minusNum = 300;
                    soundManager.Sound(2);
                    soundManager.Sound(2);
                    soundManager.Sound(2);
                    soundManager.Sound(10);
                    Debug.Log("주사기 들기 감점");
                }

            }

            if (isDo)
            {
                tag = "Untagged";
                GetComponent<Rigidbody>().isKinematic = true;
                transform.SetParent(tempTr);
                transform.localPosition = new Vector3(0.028f, -0.13f, 0.026f);
                transform.localEulerAngles = new Vector3(174.3f, 60f, 0.2f);
                transform.localScale = Vector3.one * 2.0f;
                functionState[5] = false;

            }

        }
    }

    void VaccumInsert()
    {

        tempTr = syringe.parent;
        syringe.parent = vaccum;
        syringe.localPosition = pos;
        syringe.localEulerAngles = rot;
        syringe.parent = tempTr;


        if (syringe.parent != null)
        {

            if (tempTr.name == "CustomHandRight")
            {
                bool value2 = OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.Touch);
                bool value3 = OVRInput.Get(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch);
                if (value2 || value3)
                {
                    minusNum++;
                    front.localPosition = front.localPosition + Vector3.up * +0.0035f;
                    front.localScale = front.localScale + Vector3.up * -0.0035f;
                    back.localPosition = back.localPosition + Vector3.up * 0.00075f;
                    back.localScale = back.localScale + Vector3.up * 0.00075f;

                }
            }
            else if (tempTr.name == "CustomHandLeft")
            {
                bool value1 = OVRInput.Get(OVRInput.Button.Three, OVRInput.Controller.Touch);
                bool value2 = OVRInput.Get(OVRInput.Button.Four, OVRInput.Controller.Touch);
                bool value3 = OVRInput.Get(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.LTouch);
                if (value1 || value2 || value3)
                {
                    minusNum++;
                    front.localPosition = front.localPosition + Vector3.up * +0.0035f;
                    front.localScale = front.localScale + Vector3.up * -0.0035f;
                    back.localPosition = back.localPosition + Vector3.up * 0.00075f;
                    back.localScale = back.localScale + Vector3.up * 0.00075f;
                }

            }

        }

        if (minusNum > 50 && minusNum2 < 5)
        {
            soundManager.Sound(0);
            soundManager.Sound(0);
            minusNum -= 50;
            minusNum2++;
        }
        else if (minusNum > 50 && minusNum2 == 5)
        {
            soundManager.Sound(4);
            DrawingMgr.drawing.VaccumTube();
            functionState[7] = false;
        }
    }

    #endregion




}
