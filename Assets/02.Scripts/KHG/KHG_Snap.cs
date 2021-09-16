using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHG_Snap : MonoBehaviour
{

    public enum ObjectType
    {
        None,
        Needle,
        Tourniquet,
        IVPole,
        Sap,
        Rubber,

    }

    public bool isDo = false;

    bool isLeft = false;
    public bool isEnd = false;

    Material mat;

    public GameObject bloodEfx;
    public GameObject needle2;
    public GameObject line;
    public MeshRenderer[] band;
    public ObjectType _objectType = ObjectType.None;

    private Animator anim;



    int animCount = -1;

    void Start()
    {
        anim = GameObject.FindGameObjectWithTag("Patient").GetComponent<Animator>();
        //! 확인
        if (_objectType == ObjectType.Rubber)
        {
            band = new MeshRenderer[3];
            band[0] = GameObject.FindGameObjectWithTag("FilmDressimg").transform.GetChild(0).GetComponent<MeshRenderer>();
            band[1] = GameObject.FindGameObjectWithTag("FilmDressimg").transform.GetChild(1).GetComponent<MeshRenderer>();
            band[2] = GameObject.FindGameObjectWithTag("FilmDressimg").transform.GetChild(2).GetComponent<MeshRenderer>();


            band[0].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            band[1].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            band[2].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

            band[0].enabled = false;
            band[1].enabled = false;
            band[2].enabled = false;



        }
    }

    void Update()
    {
        if (isEnd && _objectType == ObjectType.Tourniquet && transform.parent == null)
        {
            isEnd = false;
            //!엔딩
            Debug.Log("Ending");
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        Debug.Log(coll.name);

        if (_objectType == ObjectType.Needle)
        {
            if (!isDo && coll.gameObject.name == "Arm_Snap")
            {

                transform.parent.GetComponent<KHG_Needle>().NeedleSnap();
                isDo = true;
                Debug.Log("Arm to Snap");

                // 다음 단계 시작 : 주사 각도
                InjectionMgr.injection.InjectAngle();

            }
            else if (!isDo && coll.gameObject.name == "Fail_Snap")
            {

                Debug.Log("Fail to Snap");
                //잘못되었을때 애니메이션
                animCount++;
                if (animCount <= 0)
                {
                    anim.SetTrigger("Tremble");
                }
                else if (animCount == 1)
                {
                    anim.SetTrigger("HeadShaking");

                }
                else if (animCount >= 2)
                {
                    anim.SetTrigger("BedCrush");
                }


                Quaternion rot = needle2.transform.rotation;

                GameObject obj = Instantiate(bloodEfx, needle2.transform.position, rot);
                Destroy(obj, 2.0f);

            }
        }




        if (_objectType == ObjectType.Tourniquet)
        {

            if (!isDo && coll.gameObject.name == "Tor_Snap")
            {

                Debug.Log("SNAP");
                gameObject.GetComponent<BoxCollider>().enabled = false;
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<KHG_Grabble>().grabByState = KHG_Grabble.GrabByState.None;

                transform.SetParent(coll.gameObject.transform.parent);
                transform.localPosition = new Vector3(0.0162f, -0.0081f, 0.0106f);
                // transform.GetChild(0).localPosition = Vector3.zero;
                transform.localEulerAngles = new Vector3(-204.02f, -17.67f, -129.84f);


                mat = GameObject.Find("David_LOD2").GetComponent<SkinnedMeshRenderer>().materials[1];  //!숫자보정
                StartCoroutine("SetBloodLineAlpha");

                // 다음 단계 시작 : 소독
                InjectionMgr.injection.Disinfect();

                isDo = true;



            }
        }

        if (_objectType == ObjectType.IVPole)
        {
            if (!isDo && coll.name == "GrabVolumeBig")
            {
                string name = coll.transform.parent.parent.name;
                Debug.Log(name);

                if (name == "CustomHandLeft")
                {
                    //GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0, 0);
                    isDo = true;
                    isLeft = true;
                    StartCoroutine("PoleControl");
                }
                else if (name == "CustomHandRight")
                {
                    // GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0, 0);
                    isDo = true;
                    isLeft = false;
                    StartCoroutine("PoleControl");
                }

            }
        }

        if (_objectType == ObjectType.Sap)
        {
            if (!isDo && coll.gameObject.name == "Sap_Snap")
            {

                gameObject.GetComponent<BoxCollider>().enabled = false;
                coll.gameObject.GetComponent<BoxCollider>().enabled = false;
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<KHG_Grabble>().grabByState = KHG_Grabble.GrabByState.None;

                transform.SetParent(coll.gameObject.transform.parent);
                transform.localPosition = new Vector3(-0.0884362f, -0.0881395f, 1.912366f);
                transform.localEulerAngles = Vector3.zero;

                transform.Find("IVPole_Snap").GetComponent<KHG_Snap>()._objectType = KHG_Snap.ObjectType.IVPole;
                GameObject.FindGameObjectWithTag("Patient").GetComponent<BGB_Sap>().SetCurSapBag(name);
                InjectionMgr.injection.curruntSap = this.gameObject;

                isDo = true;

            }
        }

        if (_objectType == ObjectType.Rubber)
        {
            if (!isDo && coll.gameObject.name == "Rubber_Snap")
            {

                gameObject.GetComponent<BoxCollider>().enabled = false;
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<KHG_Grabble>().grabByState = KHG_Grabble.GrabByState.None;

                transform.SetParent(coll.gameObject.transform.parent);
                transform.localPosition = Vector3.zero;
                transform.localEulerAngles = Vector3.zero;
                isDo = true;


                float posZ = transform.parent.Find("Needle_2").position.z;

                // float bandZ = band[0].transform.GetChild(2).position.z;

                //0.019
                if (posZ < 1.75f)
                {
                    ChangeRubber(0);
                }
                else if (posZ < 1.85f)
                {
                    ChangeRubber(1);
                }
                else
                {
                    ChangeRubber(2);
                }

                // 다음 단계 시작 : 수액 속도
                InjectionMgr.injection.SapSpeed();




            }
        }
    }

    void OnTriggerExit()
    {
        if (isDo && _objectType == ObjectType.IVPole)
        {
            GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0, 0.35f);
            StopCoroutine("PoleControl");
            isDo = false;
        }
    }

    void LineCreate()
    {

        GameObject obj = Instantiate(line);
        //Transform trline = transform.Find("Line_Snap");
        obj.GetComponent<KHG_Line>().SetLine();


        //지혈대 초기화
        GameObject tourniquet = GameObject.Find("Tourniquet");
        tourniquet.GetComponent<BoxCollider>().enabled = true;
        tourniquet.GetComponent<Rigidbody>().useGravity = true;
        tourniquet.GetComponent<KHG_Grabble>().grabByState = KHG_Grabble.GrabByState.All;
        tourniquet.GetComponent<KHG_Snap>().isEnd = true;
    }

    void ChangeRubber(int num)
    {
        StartCoroutine("ChangeRubberAlpha", num);

    }



    IEnumerator SetBloodLineAlpha()
    {
        for (int i = 0; i < 10; i++)
        {
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.1f * i);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator PoleControl()
    {
        Vector2 value;
        float index, hand;
        WaitForSeconds ws = new WaitForSeconds(0.3f);
        while (true)
        {
            if (isLeft)
            {
                value = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
                index = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.Touch);
                hand = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch);
            }
            else
            {
                value = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
                index = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger, OVRInput.Controller.Touch);
                hand = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger, OVRInput.Controller.Touch);
            }



            //! +,- 효과주기
            BGB_Sap sap = GameObject.FindWithTag("Patient").GetComponent<BGB_Sap>();

            if (index >= 0.3f || hand >= 0.3f)
            {
                GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0, 0);
                if (value.y >= 0.3f)
                {
                    sap.UpdateSpeed(1);
                    Debug.Log("++");
                }
                else if (value.y <= -0.3f)
                {
                    sap.UpdateSpeed(-1);
                    Debug.Log("--");
                }

            }
            else
            {
                GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0, 0.35f);
            }
            yield return ws;
        }
    }

    IEnumerator ChangeRubberAlpha(int num)
    {
        WaitForSeconds ws = new WaitForSeconds(0.3f);

        MeshRenderer band1 = band[num];
        MeshRenderer band2 = band[num].transform.GetChild(0).GetComponent<MeshRenderer>();

        GameObject Film = GameObject.FindGameObjectWithTag("FilmDressimg");
        for (int i = 0; i < Film.transform.childCount; i++)
        {
            if (i != num)
            {
                Film.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        //!
        Material[] bandMats = new Material[5];
        bandMats[0] = band1.material;
        bandMats[1] = band2.materials[0];
        bandMats[2] = band2.materials[1];
        bandMats[3] = band2.materials[2];
        bandMats[4] = band2.materials[3];

        Material[] CatheterMats = transform.parent.GetComponent<MeshRenderer>().materials;
        Material[] RubberMats = GetComponent<MeshRenderer>().materials;

        foreach (Material mat in bandMats)
        {
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0);
        }

        band1.enabled = true;
        band2.enabled = true;
        for (int i = 0; i < 11; i++)
        {

            foreach (Material mat in bandMats)
            {
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.1f * i);
            }
            foreach (Material mat in CatheterMats)
            {
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1.0f - 0.1f * i);
            }
            foreach (Material mat in RubberMats)
            {
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1.0f - 0.1f * i);
            }
            yield return ws;
        }

        Invoke("LineCreate", 0.1f);

    }

}
