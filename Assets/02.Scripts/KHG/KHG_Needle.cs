using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHG_Needle : MonoBehaviour
{

    public bool isSnaped = false;
    public bool isStabed = false;


    public Vector3 pos = Vector3.zero;
    public Quaternion rot = Quaternion.identity;
    public Quaternion rot2;


    private Transform stylet;

    private BoxCollider zelcoColl;
    private Collider styletColl;
    private Collider needleColl;

    public GameObject needle;
    public GameObject snapPoint;
    public GameObject rubberPoint;

    private Animator anim;



    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        // isSnaped = false;
        // isStabed = false;

        rubberPoint.SetActive(false);
        zelcoColl = GetComponent<BoxCollider>();
        zelcoColl.size = new Vector3(0.03f, 0.03f, 0.4f);
        zelcoColl.center = new Vector3(0, 0, -0.2f);
        stylet = transform.Find("Stylet");
        styletColl = stylet.gameObject.GetComponent<BoxCollider>();
        needleColl = transform.Find("Needle").GetComponent<BoxCollider>();
        styletColl.enabled = false;

        // this.GetComponent<KHG_Grabble>().grabByState = KHG_Grabble.GrabByState.All;
        // stylet.GetComponent<KHG_Grabble>().grabByState = KHG_Grabble.GrabByState.None;

        // this.transform.position = new Vector3(100f, 0.05f, 0);
        // this.transform.rotation = Quaternion.Euler(0, 0, 0);



    }




    // Update is called once per frame
    void Update()
    {
        if (isSnaped)
        {
            if (!isStabed) transform.position = pos;

            //찌르기 액션
            if (transform.parent == null && !isStabed) //&& zelcoColl.enabled
            {


                isStabed = true;
                Debug.Log("needle:" + needle.transform.position);
                Debug.Log("default:" + transform.position);
                transform.position = pos;
                rot = transform.rotation;

                this.GetComponent<BoxCollider>().enabled = false;
                this.GetComponent<Rigidbody>().isKinematic = true;
                this.GetComponent<Rigidbody>().useGravity = false;
                this.GetComponent<KHG_Grabble>().grabByState = KHG_Grabble.GrabByState.None;

                stylet.GetComponent<KHG_Grabble>().grabByState = KHG_Grabble.GrabByState.All;
                styletColl.enabled = true;
                //stylet.SetParent(null);
                //zelcoColl.enabled = false; 

                // gameObject.GetComponent<MeshRenderer>().material.color /= 1.2f;
                // styletColl.gameObject.GetComponent<MeshRenderer>().material.color /= 1.2f;
                Debug.Log("rot:" + rot.eulerAngles);

                //Temp
                if (rot.eulerAngles.x <= 45 && rot.eulerAngles.x >= 15 && rot.eulerAngles.y < 55 && rot.eulerAngles.y > -30)
                {
                    Debug.Log("Good Needle");

                    // 다음 단계 시작 : 카테터 분리
                    InjectionMgr.injection.SeparateCatheter();
                }
                else
                {
                    Debug.Log("Bad Needle");
                    // 점수 --
                    InjectionMgr.injection.MinusAngleScore();

                    anim = GameObject.FindGameObjectWithTag("Patient").GetComponent<Animator>();
                    anim.SetTrigger("WrongAngle");

                    InjectionMgr.injection.CreateCatheter();
                    Destroy(this.gameObject);

                }


            }


            if (isStabed)
            {


                transform.localRotation = rot;
                if (!stylet) return;

                float dis = Vector3.Distance(stylet.position, pos);
                if (dis < 0.1f)
                {
                    snapPoint.transform.SetParent(this.transform);
                    if (snapPoint.transform.localPosition.z > 0)
                    {
                        snapPoint.transform.localPosition = Vector3.zero;
                    }
                    snapPoint.transform.localPosition = new Vector3(0, 0, snapPoint.transform.localPosition.z);

                    stylet.transform.position = snapPoint.transform.position;
                    snapPoint.transform.SetParent(stylet);


                }

                if (stylet && dis > 0.3f)
                {
                    // 카테터 뒷부분 분리됨
                    // 다음 단계 시작 : 고무관 연결
                    InjectionMgr.injection.ConnectRubber();

                    stylet.gameObject.GetComponent<KHG_Grabble>().destroy = true;
                    rubberPoint.SetActive(true);

                }

            }
        }
    }

    public void NeedleSnap()
    {

        pos = transform.position;
        zelcoColl.size = new Vector3(0.03f, 0.03f, 0.2f);
        zelcoColl.center = new Vector3(0, 0, -0.12f);
        isSnaped = true;


    }


    // void OnTriggerEnter(Collider coll)
    // {

    //     if (!isSnaped && coll.gameObject.name == "Arm_Snap")
    //     {
    //         Debug.Log("OnTriggerEnter");


    //         //gameObject.GetComponent<KHG_Grabble>().grabByState = KHG_Grabble.GrabByState.None;
    //         pos = transform.position;
    //         rot2 = coll.transform.rotation;
    //         // gameObject.GetComponent<MeshRenderer>().material.color *= 1.2f;
    //         // styletColl.gameObject.GetComponent<MeshRenderer>().material.color *= 1.2f;

    //         isSnaped = true;

    //     }
    // }

}
