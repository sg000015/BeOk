using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHG_Control : MonoBehaviour
{

    public enum GrabState { None, Pinch, Grab }

    public GameObject OtherController;
    public Transform controllerTr;

    public Transform grabbedObject;
    public Transform currentGrabbedObject;
    private List<GameObject> grabbedGameObjects;

    bool isTouched = false;
    bool isGrabbed = false;

    float grabBegin = 0.5f; // bigger than 0.2

    public OVRInput.Controller controller;

    float m_prevFlex;
    float prevFlex;

    float m_prevFlex_Grab;
    float prevFlex_Grab;

    bool Right;
    bool Left;

    public GrabState grabState = 0;

    void Start()
    {


        controllerTr = transform;
        grabbedGameObjects = new List<GameObject>();

        if (controller == OVRInput.Controller.LTouch) { Left = true; }
        else { Right = true; }

    }

    int i;
    void Update()
    {
        prevFlex = m_prevFlex;
        prevFlex_Grab = m_prevFlex_Grab;
        // Update values from inputs
        if (Left)
        {
            m_prevFlex = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.Touch);
            m_prevFlex_Grab = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch);
        }
        else if (Right)
        {
            m_prevFlex = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger, OVRInput.Controller.Touch);
            m_prevFlex_Grab = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger, OVRInput.Controller.Touch);
        }




        bool ThumbTouch = false;
        if (Left)
        {
            ThumbTouch = OVRInput.Get(OVRInput.Touch.PrimaryThumbstick, OVRInput.Controller.Touch);
        }
        else if (Right)
        {
            ThumbTouch = OVRInput.Get(OVRInput.Touch.SecondaryThumbstick, OVRInput.Controller.Touch);
        }



        if (ThumbTouch)
        {
            Debug.Log("TrueTrueTrue");
        }


        //그랩시작
        if (grabState != GrabState.Grab)
        {
            //엄지,검지버튼
            // if ((m_prevFlex >= grabBegin) && (prevFlex < grabBegin) && ThumbTouch)    //그랩시작 순간
            if ((m_prevFlex >= 0.35f && m_prevFlex < 0.99f) && ThumbTouch)    //그랩시작 순간
            {
                grabState = (GrabState)1;
                if (!isGrabbed)
                {
                    GrabBegin();

                }
            }
            else if (m_prevFlex <= 0.3f || !ThumbTouch)   //그랩 떼는 순간, 그랩 시점이랑 차이를 둬서 안정적으로 그랩
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
            //그랩버튼
            if (m_prevFlex_Grab >= 0.35f && m_prevFlex_Grab < 0.99f)     //그랩시작 순간
            {
                grabState = (GrabState)2;
                if (!isGrabbed)
                {
                    GrabBegin();
                }
            }
            else if (m_prevFlex_Grab <= 0.3f)   //그랩 떼는 순간, 그랩 시점이랑 차이를 둬서 안정적으로 그랩
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
        if (grabbedObject != null)
        {
            var _grabByState = grabbedObject.GetComponent<KHG_Grabble>().grabByState;

            if (grabState == GrabState.Pinch && (_grabByState == KHG_Grabble.GrabByState.All || _grabByState == KHG_Grabble.GrabByState.Pinch))
            {
                currentGrabbedObject = grabbedObject;
                currentGrabbedObject.SetParent(controllerTr);
                currentGrabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                isGrabbed = true;
                // isGrabbed = true;

            }
            else if (grabState == GrabState.Grab && (_grabByState == KHG_Grabble.GrabByState.All || _grabByState == KHG_Grabble.GrabByState.Grab))
            {
                currentGrabbedObject = grabbedObject;
                currentGrabbedObject.SetParent(controllerTr);
                currentGrabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                isGrabbed = true;
                // isGrabbed = true;

            }
        }


    }

    void GrabEnd()
    {
        if (currentGrabbedObject != null)
        {
            if (OtherController.GetComponent<KHG_Control>().currentGrabbedObject != currentGrabbedObject)
            {
                currentGrabbedObject.SetParent(null);
                currentGrabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            }

            isGrabbed = false;
            currentGrabbedObject = null;

        }

    }






    //다중 트리거Enter, 같은물체 양손 트리거
    //1번물체 트리거Enter -> 2번물체 트리거Enter -> 1번문체 트리거Exit  일시, grabbedObject = null발동

    int grabCount = 0;
    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "GrabObject")
        {

            //coll.gameObject.GetComponent<MeshRenderer>().material.color *= 1.3f;
            Material mat = coll.gameObject.GetComponent<MeshRenderer>()?.material;
            mat.color *= 1.3f;
        }

        if (!isGrabbed)
        {
            if (coll.tag == "GrabObject")
            {
                grabbedObject = coll.transform;
                grabCount++;
            }
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.tag == "GrabObject")
        {
            //coll.gameObject.GetComponent<MeshRenderer>().material.color *= 1.3f;
            Material mat = coll.gameObject.GetComponent<MeshRenderer>()?.material;
            mat.color /= 1.3f;
        }
        if (!isGrabbed)
        {
            grabCount--;
            if (grabCount == 0)
            {
                grabbedObject = null;
            }
        }

    }



}

