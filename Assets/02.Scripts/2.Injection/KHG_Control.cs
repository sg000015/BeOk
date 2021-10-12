using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class KHG_Control : MonoBehaviour
{


    public enum GrabState { None, Pinch, Grab }

    public GameObject OtherController;
    public Transform controllerTr;

    public Transform grabbedObject;
    public Transform currentGrabbedObject;
    private List<GameObject> grabbedGameObjects;

    bool isTouched = false;
    public bool isGrabbed = false;

    float grabBegin = 0.5f; // bigger than 0.2

    public OVRInput.Controller controller;

    float m_prevFlex;
    float prevFlex;

    float m_prevFlex_Grab;
    float prevFlex_Grab;

    bool Right;
    bool Left;

    public GrabState grabState = 0;

    public string[] canGrabObjects = {"Tourniquet","AlcoholCotton","Sap_Normal Saline","Sap_5% Dextrose", "Sap_0.45% Saline","IVPole","Rubber","Catheter_Res","Stylet"};

    public TMPro.TMP_Text texttt;

    PhotonView pv;
    void Start()
    {
        texttt = GameObject.Find("Text (TMP)_ef")?.GetComponent<TMPro.TMP_Text>();
        if(SceneManager.GetActiveScene().name == "Ward-Injection")
        {
            pv = gameObject.GetPhotonView();

            if (pv.IsMine)
            {
                controllerTr = transform;
                grabbedGameObjects = new List<GameObject>();

                if (controller == OVRInput.Controller.LTouch) { Left = true; }
                else { Right = true; }
            }
        }
        else if(SceneManager.GetActiveScene().name == "Ward-BloodCollection-BACKUP")
        {
            pv = gameObject.GetPhotonView();
            if (pv.IsMine)
            {
                controllerTr = transform;
                grabbedGameObjects = new List<GameObject>();

                if (controller == OVRInput.Controller.LTouch) { Left = true; }
                else { Right = true; }
            }
        }


        
    }

    // public void GrabInit()
    // {
    //     pv.RPC(nameof(GrabInitRPC), RpcTarget.AllViaServer);
    // }

    // [PunRPC]
    // public void GrabInitRPC()
    // {
    //         controllerTr = transform;
    //         grabbedGameObjects = new List<GameObject>();
    //         if (controller == OVRInput.Controller.LTouch) { Left = true; }
    //         else { Right = true; }
    // }

    int i;
    void Update()
    {
        // if (!PhotonNetwork.IsMasterClient) return;

        if (isGrabbed && currentGrabbedObject == null)
        {
            isGrabbed = false;
            grabCount = 0;
        }

        bool ThumbTouch = false;
        prevFlex = m_prevFlex;
        prevFlex_Grab = m_prevFlex_Grab;
        // Update values from inputs
        if (Left)
        {
            m_prevFlex = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.Touch);
            m_prevFlex_Grab = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch);
            ThumbTouch = OVRInput.Get(OVRInput.Touch.PrimaryThumbstick, OVRInput.Controller.Touch);

        }
        else if (Right)
        {
            m_prevFlex = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger, OVRInput.Controller.Touch);
            m_prevFlex_Grab = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger, OVRInput.Controller.Touch);
            ThumbTouch = OVRInput.Get(OVRInput.Touch.SecondaryThumbstick, OVRInput.Controller.Touch);

        }


        //그랩시작
        if (grabState != GrabState.Grab)
        {
            //엄지,검지버튼
            // if ((m_prevFlex >= grabBegin) && (prevFlex < grabBegin) && ThumbTouch)    //그랩시작 순간
            if ((m_prevFlex > 0f))    //그랩시작 순간
            {
                grabState = (GrabState)1;
                if (!isGrabbed)
                {
                    GrabBegin();
                }
            }
            else if (m_prevFlex == 0f)   //그랩 떼는 순간, 그랩 시점이랑 차이를 둬서 안정적으로 그랩
            {
                grabState = (GrabState)0;

                if (isGrabbed)
                {
                    GrabEnd();
                }
            }
        }
        if (grabState != GrabState.Pinch)
        {
            //그랩버튼 m_prevFlex_Grab>= 0.35f && m_prevFlex_Grab < 0.99f
            if (m_prevFlex_Grab > 0)     //그랩시작 순간
            {
                grabState = (GrabState)2;
                if (!isGrabbed)
                {
                    GrabBegin();
                }
            }
            else if (m_prevFlex_Grab== 0)   //그랩 떼는 순간, 그랩 시점이랑 차이를 둬서 안정적으로 그랩  <= 0.3f
            {
                grabState = (GrabState)0;
                if (isGrabbed)
                {
                    GrabEnd();
                }
            }
        }

        //손에서
        // if (grabbedObject.parent ? true : false)
        // {
        //     grabbedObject = null;
        // }


        // if (isGrabbed)
        // {
        //     i++;
        //     if (i > 50)
        //     {
        //         OVRInput.SetControllerVibration(0.5f, 0.5f, controller);
        //         i = 0;
        //     }
        // }

    }
    void GrabBegin()
    {
        // Debug.Log("Grab Check : " + Physics.OverlapSphere(this.transform.position, 0.13f).Length);

        if (Physics.OverlapSphere(this.transform.position, 0.13f).Length == 2) { grabCount = 0; grabbedObject = null; currentGrabbedObject = null; isGrabbed = false; } //버그방지

        if (grabbedObject != null)
        {
            //!
            if (grabbedObject.GetComponent<KHG_Grabble>().isGrab) { return; }
            if (grabbedObject.tag != "GrabObject") { return; }

            var _grabByState = grabbedObject.GetComponent<KHG_Grabble>().grabByState;

            if (grabState == GrabState.Pinch && (_grabByState == KHG_Grabble.GrabByState.All || _grabByState == KHG_Grabble.GrabByState.Pinch))
            {
                currentGrabbedObject = grabbedObject;
                grabbedObject.GetComponent<KHG_Grabble>().isGrab = true;

                isGrabbed = true;

                pv.RPC(nameof(PinchRPC), RpcTarget.AllViaServer, currentGrabbedObject.name);

            }
            else if (grabState == GrabState.Grab && (_grabByState == KHG_Grabble.GrabByState.All || _grabByState == KHG_Grabble.GrabByState.Grab))
            {
                currentGrabbedObject = grabbedObject;
                grabbedObject.GetComponent<KHG_Grabble>().isGrab = true;
                isGrabbed = true;

                pv.RPC(nameof(GrabRPC), RpcTarget.AllViaServer, currentGrabbedObject.name);
            }
        }
    }

    [PunRPC]
    void PinchRPC(string objName)
    {
        currentGrabbedObject = GameObject.Find(objName).transform;

        currentGrabbedObject.GetComponent<Rigidbody>().isKinematic = true;
        currentGrabbedObject.SetParent(transform);
        //currentGrabbedObject.GetComponent<KHG_Grabble>().isGrab = true;
        // isGrabbed = true;
        //!
    }

    [PunRPC]
    void GrabRPC(string objName)
    {
        currentGrabbedObject = GameObject.Find(objName).transform;

        currentGrabbedObject.GetComponent<Rigidbody>().isKinematic = true;
        currentGrabbedObject.SetParent(transform);
        //currentGrabbedObject.GetComponent<KHG_Grabble>().isGrab = true;
        // isGrabbed = true;
        //!
    }

    void GrabEnd()
    {
        // Debug.Log("ChildCount" + transform.childCount);

        // if (transform.childCount == 1) { grabCount = 0; }

        if (currentGrabbedObject != null)
        {
            if (OtherController.GetComponent<KHG_Control>().currentGrabbedObject != currentGrabbedObject)
            {
                if (currentGrabbedObject.parent == controllerTr)
                {
                    pv.RPC(nameof(GrabParentRPC), RpcTarget.AllViaServer, currentGrabbedObject.name);
                }
            }

            isGrabbed = false;
            pv.RPC(nameof(CurrentNullRPC), RpcTarget.AllViaServer);
            //!
            grabCount = 0;
            grabbedObject.GetComponent<KHG_Grabble>().isGrab = false;

        }

    }

    [PunRPC]
    void CurrentNullRPC()
    {
        currentGrabbedObject = null;
    }


    //! 이름 넘기기
    [PunRPC]
    void GrabParentRPC(string objName)
    {
        currentGrabbedObject = GameObject.Find(objName).transform;

        currentGrabbedObject.GetComponent<Rigidbody>().isKinematic = false;
        // currentGrabbedObject.SetParent(null);

        currentGrabbedObject.transform.parent = null;
        //currentGrabbedObject.GetComponent<KHG_Grabble>().isGrab = false;
    }




    //다중 트리거Enter, 같은물체 양손 트리거
    //1번물체 트리거Enter -> 2번물체 트리거Enter -> 1번문체 트리거Exit  일시, grabbedObject = null발동

    public int grabCount = 0;
    void OnTriggerEnter(Collider coll)
    {
        // pv?.RPC(nameof(OnTriggerEnterRPC), RpcTarget.AllViaServer, coll.name);

        if (coll.tag == "GrabObject")
        {

            if (coll.GetComponent<KHG_Grabble>().grabByState != KHG_Grabble.GrabByState.None)
            {
                //coll.gameObject.GetComponent<MeshRenderer>().material.color *= 1.3f;
                try
                {
                    Material mat = coll.gameObject.GetComponent<MeshRenderer>()?.material;
                    mat.color *= 1.2f;
                }
                catch { }
            }

        }

        if (!isGrabbed)
        {
            if (coll.tag == "GrabObject")
            {
                // if (coll.GetComponent<KHG_Grabble>().grabByState != KHG_Grabble.GrabByState.None)
                {
                    // texttt.text = "트리거엔터!! 그리고 그랩오브젝트 활성화!";
                    grabbedObject = coll.transform;
                    grabCount++;
                    coll.GetComponent<KHG_Grabble>().isExit = false;

                }
            }
        }
    }
    void OnTriggerExit(Collider coll)
    {
    //    pv?.RPC(nameof(OnTriggerExitRPC), RpcTarget.AllViaServer, coll.name);
            if (coll.tag == "GrabObject")
        {
            if (coll.GetComponent<KHG_Grabble>().grabByState != KHG_Grabble.GrabByState.None)
            {
                try
                {
                    //coll.gameObject.GetComponent<MeshRenderer>().material.color *= 1.3f;
                    Material mat = coll.gameObject.GetComponent<MeshRenderer>()?.material;
                    mat.color /= 1.2f;
                }
                catch { }
            }

        }
        if (!isGrabbed)
        {
            if (coll.GetComponent<KHG_Grabble>())
            {
                // if (coll.GetComponent<KHG_Grabble>().grabByState != KHG_Grabble.GrabByState.None)
                {
                    if (grabCount > 0)
                    {
                        grabCount--;
                    }
                    coll.GetComponent<KHG_Grabble>().isExit = true;
                    if (grabCount == 0)
                    {
                        texttt.text = "그랩오브젝트 비활성화!";

                        grabbedObject = null;
                    }
                }

            }
        }
    }

    void OnTriggerExitRPC(Collider coll)
    {

    }



}

