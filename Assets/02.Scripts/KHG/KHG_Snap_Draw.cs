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
    public bool isDo = false;

    public ObjectType _objectType;
    private SoundManager soundManager;


    //함수 실행 여부
    bool[] functionState = new bool[10];

    public GameObject bloodEfx;

    Transform syringe;
    Transform front;
    Transform back;
    Transform point;
    Transform pull;
    Transform pullback;
    Transform tempTr;
    Transform blood;
    public Transform vaccum;
    Transform vialSnap;
    Transform vialSnap2;

    Vector3 rot;
    Vector3 reset;
    Vector3 backReset;
    Vector3 pos;


    float dis;
    float lastDis;

    int minusNum = 0;
    int minusNum2 = 0;
    int airCount = 0;



    bool check = false;
    bool airCheck;
    bool isFirst = false;

    // bool syringeCoverGrabCheck = false;


    void OnTriggerEnter(Collider coll)
    {
        Debug.Log(coll.name);

        if (_objectType == ObjectType.AlcoholCotton)
        {
            if (coll.name == "VirusBody")
            {
                coll.GetComponentInParent<VirusMgr>().AlcoholCottonEnter();

            }

            else if (functionState[5])
            {
                if (coll.name == "Needle_Point")
                {
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
                StartCoroutine("TimeCheck");
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

                if (needleAngle.x > 355f || needleAngle.x < 305 || needleAngle.y < 155 || needleAngle.y > 205)
                {
                    Debug.Log("감점-20 \nDegree X : 10~40도 (350 ~ 310 사이)\nDegree Y : -20~20도 (160 ~ 200 사이)");
                    DrawingMgr.drawing.scoreList[0] -= 20;
                }
                else if (needleAngle.x > 350f || needleAngle.x < 320 || needleAngle.y < 160 || needleAngle.y > 200)
                {
                    Debug.Log("감점-10 \nDegree X : 10~40도 (350 ~ 310 사이)\nDegree Y : -20~20도 (160 ~ 200 사이)");
                    DrawingMgr.drawing.scoreList[0] -= 10;
                }

                syringe = transform.parent.parent;
                syringe.GetComponent<Rigidbody>().isKinematic = true;
                // syringe.tag = "Untagged";
                pos = syringe.position;
                rot = syringe.eulerAngles;
                functionState[2] = true;
                syringe.parent = null;
                syringe.GetComponent<MeshRenderer>().material.color /= 1.2f;

                coll.GetComponent<VesselColor>().check = true;
                DrawingMgr.drawing.arrow.gameObject.SetActive(false);
                coll.gameObject.SetActive(false);
                DrawingMgr.drawing.SyringeArea();
                soundManager.Sound(4);
                isDo = true;


            }
            else if (!isDo && coll.name == "Fail_Snap")
            {
                //TODO 피튀기기
                // transform.localEulerAngles = Vector3.up * 180f;
                GameObject obj = Instantiate(bloodEfx, transform.position, transform.rotation);
                // transform.localEulerAngles = Vector3.zero;
                Destroy(obj, 2.0f);
                DrawingMgr.drawing.scoreList[0] -= 5;
                soundManager.Sound(9);
                isDo = true;
                Invoke("SetIsDo", 0.5f);

            }
            else if (coll.name == "Vaccum_Snap")
            {
                if (functionState[6])
                {
                    vaccum = coll.transform;
                    front = transform.parent.parent.Find("Blood");
                    back = coll.transform.parent.Find("Blood");
                    soundManager.Sound(4);
                    minusNum = 0;
                    minusNum2 = 0;
                    if (vialSnap == null)
                    {
                        vialSnap = coll.transform;
                        if (coll.transform.parent.GetComponent<VaccumTubeMgr>().a > 3)
                        {
                            DrawingMgr.drawing.scoreList[3] -= 20;
                        }
                    }
                    else
                    {
                        vialSnap2 = coll.transform;
                    }
                    functionState[6] = false;
                    functionState[7] = true;
                    //! 동작수정
                    syringe.parent = vaccum;
                    syringe.localPosition = pos;
                    syringe.localEulerAngles = rot;
                    syringe.tag = "Untagged";
                    syringe.GetComponent<MeshRenderer>().material.color /= 1.2f;
                    syringe.GetComponent<BoxCollider>().enabled = false;
                    syringe.GetComponent<KHG_Grabble>().grabByState = KHG_Grabble.GrabByState.None;

                    pullback = transform.parent.parent.Find("Syringe_Back");
                    pull = pullback.Find("Pull_Snap");
                    pull.GetComponent<BoxCollider>().enabled = true;
                    pull.tag = "GrabObject";


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
                if (airCount > 8)
                {
                    soundManager.Sound(4);
                    DrawingMgr.drawing.BloodShake(transform.parent);
                    GetComponent<BoxCollider>().enabled = false;
                }
            }
        }
        else if (!isDo && _objectType == ObjectType.VaccumTube)
        {
            if (functionState[8] && coll.name == "Vial_Snap1")
            {
                int number = GetComponent<VaccumTubeMgr>().a;
                if (number < 3)
                {
                    transform.parent = coll.transform.parent;
                    tag = "Untagged";
                    GetComponent<Rigidbody>().isKinematic = true;
                    switch (number)
                    {
                        case 0:
                            transform.localPosition = new Vector3(-0.63f, 0.18f, 0);
                            break;
                        case 1:
                            transform.localPosition = new Vector3(-0.46f, 0.18f, 0);
                            break;
                        case 2:
                            transform.localPosition = new Vector3(-0.29f, 0.18f, 0);
                            break;
                        default:
                            break;
                    }

                    transform.localEulerAngles = Vector3.up * 90f;
                    isDo = true;
                    soundManager.Sound(4);
                    functionState[8] = false;
                    DrawingMgr.drawing.Finish();
                }

            }
            else if (functionState[8] && coll.name == "Vial_Snap2")
            {
                int number = GetComponent<VaccumTubeMgr>().a;
                if (number >= 3)
                {
                    transform.parent = coll.transform.parent;
                    tag = "Untagged";
                    GetComponent<Rigidbody>().isKinematic = true;
                    switch (number)
                    {
                        case 3:
                            transform.localPosition = new Vector3(-0.12f, 0.18f, 0);
                            break;
                        case 4:
                            transform.localPosition = new Vector3(0.128f, 0.18f, 0);
                            break;
                        case 5:
                            transform.localPosition = new Vector3(0.295f, 0.18f, 0);
                            break;
                        default:
                            break;
                    }
                    transform.localEulerAngles = Vector3.up * 90f;
                    isDo = true;
                    soundManager.Sound(4);
                    functionState[8] = false;
                    DrawingMgr.drawing.Finish();
                }

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

    void SetIsDo()
    {
        isDo = false;
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
            transform.position = new Vector3(-0.206f, 0.78f, -0.587f);
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

            // Material mat = back.Find("Group_004").GetComponent<MeshRenderer>().material;
            Material mat = syringe.GetComponent<MeshRenderer>().material;
            StartCoroutine("SyringePick", mat);

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


            check = false;
            Material mat = GetComponent<MeshRenderer>().material;
            StartCoroutine("HightedColor", mat);
            StartCoroutine("ObjectPick", transform);
        }
    }

    public void DrawingStart()
    {
        if (_objectType == ObjectType.Pull)
        {
            tag = "GrabObject";
            syringe = transform.parent.parent;
            back = transform.parent;
            // minusNum = 100;
            rot = new Vector3(90, -90f, 0);
            reset = new Vector3(0, 0.363f, 4f);
            backReset = new Vector3(0.383f, 1.31f, 0);
            isFirst = true;
            blood = syringe.Find("Blood");
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
            // minusNum = 100;
            functionState[4] = true;
        }

    }

    public void AlcoholCottonActiveStart(Transform _syringe, Transform _arm)
    {

        syringe = _syringe;
        tempTr = _arm;
        // minusNum = 100;
        functionState[5] = true;

    }

    public void SyringeOffStart()
    {
        syringe = transform.parent.parent;
        syringe.GetComponent<BoxCollider>().enabled = true;
        syringe.GetComponent<KHG_Grabble>().grabByState = KHG_Grabble.GrabByState.All;

        syringe.GetComponent<Rigidbody>().isKinematic = true;
        syringe.tag = "GrabObject";
        rot = Vector3.right * 180.0f;
        pos = Vector3.up * 10f;
        minusNum = 0;
        minusNum2 = 0;

        functionState[2] = false;
        functionState[6] = true;
        isDo = false;
    }

    public void ShakingStart()
    {
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
            SyringeGrabCheck();

            if (transform.parent == null)
            {
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

                tempTr = transform.parent;
                transform.parent = back;

                dis = Vector3.Distance(back.position, transform.position);

                if (dis <= 0.25f)
                {
                    back.localPosition = backReset - Vector3.up * 12 * (-0.1f + Mathf.Abs(dis));
                }

                if (!airCheck && dis > 0.25f)
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
        SyringeGrabCheck();

        // if (syringeCoverGrabCheck)
        // {
        //     syringeCoverGrabCheck = false;
        //     StartCoroutine("HightedColor", GetComponent<MeshRenderer>().material);
        //     StartCoroutine("ObjectPick", transform);
        // }

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
            if (syringe.parent != null && transform.parent != null && dis > 0.2f)
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
        syringe.GetComponent<KHG_Grabble>().grabByState = KHG_Grabble.GrabByState.None;
        syringe.GetComponent<BoxCollider>().enabled = false;

        if (_objectType == ObjectType.Pull)
        {
            minusNum2--;
            // if (syringe.parent == null)
            // {
            //     minusNum--;
            //     //!주사기 놓을시 감점(딜레이 주의) : "주사기를 잡아주세요"
            //     if (minusNum < 1)
            //     {
            //         minusNum = 300;
            //         soundManager.Sound(2);
            //         soundManager.Sound(2);
            //         soundManager.Sound(2);
            //         soundManager.Sound(10);
            //         Debug.Log("주사기 들기 감점");
            //     }

            // }
            // else
            // {
            //     minusNum = 100;
            // }

            if (transform.parent == null)
            {
                isFirst = true;
                transform.GetComponent<Rigidbody>().isKinematic = true;
                transform.GetComponent<Rigidbody>().useGravity = false;
                transform.parent = back;

                back.localPosition = backReset;
                back.localEulerAngles = rot;

                // dis = Vector3.Distance(back.position, transform.position);
                transform.localPosition = reset;
                transform.localEulerAngles = Vector3.zero;

                blood.localPosition = Vector3.up * 1.8f;
                blood.localScale = new Vector3(0.5f, 0, 0.5f);

            }

            else if ((transform.parent.name == "CustomHandRight" || transform.parent.name == "CustomHandLeft"))
            {

                // tempTr = transform.parent;

                dis = Vector3.Magnitude(back.position - transform.position);

                // Debug.Log("First dis" + dis);

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
                            if (DrawingMgr.drawing.scoreList[2] >= 10)
                            {
                                DrawingMgr.drawing.scoreList[2] -= 10;
                            }
                        }
                }
                isFirst = false;
                lastDis = dis;


                float fixdis = Mathf.Abs(dis);
                back.localPosition = backReset - Vector3.up * 15 * (-0.1f + fixdis);

                blood.localPosition = Vector3.up * (1.8f + -7.3f * (fixdis - 0.1f));
                blood.localScale = new Vector3(0.5f, 7.3f * (fixdis - 0.1f), 0.5f);

                if (dis > 0.25f)
                {

                    transform.parent = back;
                    transform.localPosition = new Vector3(0, 0.363f, 3.83f);
                    transform.localEulerAngles = Vector3.zero;
                    transform.GetComponent<Rigidbody>().isKinematic = true;
                    transform.GetComponent<Rigidbody>().useGravity = false;
                    transform.GetComponent<BoxCollider>().enabled = false;

                    soundManager.Sound(4);
                    functionState[3] = false;
                    tag = "Untagged";
                    DrawingMgr.drawing.BloodDrawing();

                }

                // transform.parent = tempTr;




            }
        }

    }

    void TorniquetRestore()
    {
        if (_objectType == ObjectType.Tourniquet)
        {

            // if (syringe.parent == null)
            // {
            //     minusNum--;
            //     //!주사기 놓을시 감점(딜레이 주의) : "주사기를 잡아주세요"
            //     if (minusNum < 1)
            //     {
            //         minusNum = 300;
            //         soundManager.Sound(2);
            //         soundManager.Sound(2);
            //         soundManager.Sound(2);
            //         soundManager.Sound(10);
            //         Debug.Log("주사기 들기 감점");
            //     }

            // }
            if (transform.parent == null)
            {

                StopCoroutine("TimeCheck");
                if (timecheck < 60)
                {

                }
                else if (timecheck < 80)
                {
                    DrawingMgr.drawing.scoreList[1] -= 5;
                }
                else if (timecheck < 100)
                {
                    DrawingMgr.drawing.scoreList[1] -= 10;
                }
                else if (timecheck < 120)
                {
                    DrawingMgr.drawing.scoreList[1] -= 15;
                }
                else
                {
                    DrawingMgr.drawing.scoreList[1] -= 20;
                }
                Debug.Log("지혈대 시간:" + timecheck);

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

            // if (syringe.parent == null)
            // {
            //     minusNum--;
            //     //!주사기 놓을시 감점(딜레이 주의) : "주사기를 잡아주세요"
            //     if (minusNum < 1)
            //     {
            //         minusNum = 300;
            //         soundManager.Sound(2);
            //         soundManager.Sound(2);
            //         soundManager.Sound(2);
            //         soundManager.Sound(10);
            //         Debug.Log("주사기 들기 감점");
            //     }

            // }

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

        // tempTr = syringe.parent;
        // syringe.parent = vaccum;
        // syringe.localPosition = pos;
        // syringe.localEulerAngles = rot;
        // syringe.parent = tempTr;

        //! 동작수정
        float _num;
        if (pull.parent != null)
        {
            if (pull.parent.name == "CustomHandRight" || pull.parent.name == "CustomHandLeft")
            {
                tempTr = pull.parent;
                pull.SetParent(pullback);
                _num = pull.transform.localPosition.z;
                pull.localPosition = new Vector3(0, 0.363f, 3.83f);
                pull.localEulerAngles = Vector3.zero;
                pull.parent = tempTr;
                if (_num < 3.83f)
                {
                    Debug.Log("MINUSNUM");
                    minusNum += 2;
                    pullback.localPosition = pullback.localPosition + Vector3.up * 0.007f;
                    front.localPosition = front.localPosition + Vector3.up * +0.0035f;
                    front.localScale = front.localScale + Vector3.up * -0.0035f;
                    back.localPosition = back.localPosition + Vector3.up * 0.0015f;
                    back.localScale = back.localScale + Vector3.up * 0.0015f;
                }

            }
        }
        else if (pull.parent == null)
        {
            pull.SetParent(pullback);
            pull.localPosition = new Vector3(0, 0.363f, 3.83f);
            pull.localEulerAngles = Vector3.zero;
        }

        // if (syringe.parent != null)
        // {

        //     if (tempTr.name == "CustomHandRight")
        //     {
        //         bool value2 = OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.Touch);
        //         bool value3 = OVRInput.Get(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch);
        //         if (value2 || value3)
        //         {
        //             minusNum++;
        //             front.localPosition = front.localPosition + Vector3.up * +0.00175f;
        //             front.localScale = front.localScale + Vector3.up * -0.00175f;
        //             back.localPosition = back.localPosition + Vector3.up * 0.00075f;
        //             back.localScale = back.localScale + Vector3.up * 0.00075f;

        //         }
        //     }
        //     else if (tempTr.name == "CustomHandLeft")
        //     {
        //         bool value1 = OVRInput.Get(OVRInput.Button.Three, OVRInput.Controller.Touch);
        //         bool value2 = OVRInput.Get(OVRInput.Button.Four, OVRInput.Controller.Touch);
        //         bool value3 = OVRInput.Get(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.LTouch);
        //         if (value1 || value2 || value3)
        //         {
        //             minusNum++;
        //             front.localPosition = front.localPosition + Vector3.up * +0.00175f;
        //             front.localScale = front.localScale + Vector3.up * -0.00175f;
        //             back.localPosition = back.localPosition + Vector3.up * 0.00075f;
        //             back.localScale = back.localScale + Vector3.up * 0.00075f;
        //         }

        //     }

        // }



        if (vialSnap2 == null)
        {
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
                vialSnap.GetComponent<BoxCollider>().enabled = false;
                functionState[7] = false;
                functionState[6] = true;

                syringe.tag = "GrabObject";
                syringe.GetComponent<BoxCollider>().enabled = true;
                syringe.GetComponent<KHG_Grabble>().grabByState = KHG_Grabble.GrabByState.All;

                pull.SetParent(pullback);
                pull.localPosition = new Vector3(0, 0.363f, 3.83f);
                pull.localEulerAngles = Vector3.zero;
                pull.GetComponent<BoxCollider>().enabled = false;
                pull.tag = "Untagged";

                DrawingMgr.drawing.VaccumTube(vialSnap.parent);
            }
        }
        else if (vialSnap2 != null)
        {
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
                vialSnap2.GetComponent<BoxCollider>().enabled = false;
                functionState[7] = false;
                syringe.tag = "GrabObject";
                syringe.GetComponent<BoxCollider>().enabled = true;
                syringe.GetComponent<KHG_Grabble>().grabByState = KHG_Grabble.GrabByState.All;

                pull.SetParent(pullback);
                pull.localPosition = new Vector3(0, 0.363f, 3.83f);
                pull.localEulerAngles = Vector3.zero;
                pull.GetComponent<BoxCollider>().enabled = false;
                pull.tag = "Untagged";

                DrawingMgr.drawing.VaccumTube(vialSnap2.parent);
            }
        }
    }

    #endregion

    #region Coroutine

    void SyringeGrabCheck()
    {
        if (_objectType == ObjectType.Pull)
        {
            if (DrawingMgr.drawing.syringeGrab)
            {
                GetComponent<BoxCollider>().enabled = true;
            }
            else
            {
                GetComponent<BoxCollider>().enabled = false;
            }
        }

        if (_objectType == ObjectType.NeedleCover)
        {
            if (DrawingMgr.drawing.syringeGrab)
            {
                GetComponent<BoxCollider>().enabled = true;
            }
            else
            {
                GetComponent<BoxCollider>().enabled = false;
            }
        }

    }



    IEnumerator SyringePick(Material _mat)
    {
        WaitForSeconds ws = new WaitForSeconds(0.05f);
        Color _color = _mat.color;
        while (true)
        {
            if (!DrawingMgr.drawing.syringeGrab)
            {
                // for (int i = 0; i < 5; i++)
                // {
                //     _mat.color = new Color(0.8f, 0.8f - 0.2f * i, 0.8f - 0.1f * i);
                //     yield return ws;
                // }
                // for (int i = 0; i < 5; i++)
                // {
                //     _mat.color = new Color(0.8f, 0.2f * i, 0.4f + 0.1f * i);
                //     yield return ws;
                // }
                yield return ws;
            }
            else
            {
                _mat.color = _color;
                Material mat = back.Find("Group_004").GetComponent<MeshRenderer>().material;
                StartCoroutine("HightedColor", mat);
                StartCoroutine("ObjectPick", transform);
                // syringeCoverGrabCheck = true;
                StopCoroutine("SyringePick");
                break;
            }

        }

    }

    IEnumerator HightedColor(Material _mat)
    {
        WaitForSeconds ws = new WaitForSeconds(0.03f);
        Color _color = _mat.color;
        while (true)
        {

            for (int i = 0; i < 5; i++)
            {
                _mat.color = new Color(0.8f, 0.8f - 0.2f * i, 0.8f - 0.1f * i);
                yield return ws;
            }
            for (int i = 1; i < 5; i++)
            {
                _mat.color = new Color(0.8f, 0.2f * i, 0.4f + 0.1f * i);
                yield return ws;
            }

            //!조건
            if (check)
            {
                _mat.color = _color;
                Debug.Log("Stop");
                StopCoroutine("HightedColor");
                break;
            }
        }
    }

    IEnumerator ObjectPick(Transform _tr)
    {
        WaitForSeconds ws = new WaitForSeconds(0.05f);
        while (true)
        {
            if (_tr.parent == null)
            {

            }
            else if (_tr.parent.name == "CustomHandRight" || _tr.parent.name == "CustomHandLeft")
            {
                check = true;
                StopCoroutine("ObjectPick");
            }
            yield return ws;

        }

    }

    int timecheck = 0;
    IEnumerator TimeCheck()
    {
        WaitForSeconds ws = new WaitForSeconds(1f);
        while (true)
        {
            timecheck++;
            yield return ws;
        }
    }


    #endregion


}
