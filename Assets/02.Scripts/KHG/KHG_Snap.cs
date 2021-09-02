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

    Material mat;

    public GameObject bloodEfx;
    public GameObject needle2;
    public GameObject line;
    public ObjectType _objectType = ObjectType.None;

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


                Quaternion rot = needle2.transform.rotation;

                GameObject obj = Instantiate(bloodEfx, needle2.transform.position, rot);
                Destroy(obj, 2.0f);

            }
        }




        if (_objectType == ObjectType.Tourniquet)
        {
            if (!isDo && coll.gameObject.name == "Tor_Snap")
            {

                gameObject.GetComponent<BoxCollider>().enabled = false;
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<KHG_Grabble>().grabByState = KHG_Grabble.GrabByState.None;

                transform.SetParent(coll.gameObject.transform.parent);
                transform.localPosition = new Vector3(0.6f, -0.32f, -0.66f);
                transform.localEulerAngles = new Vector3(50f, -25f, 45f);


                mat = coll.transform.parent.GetComponent<MeshRenderer>().materials[2];  //!숫자보정
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
                    StartCoroutine("PoleControl");
                    isDo = true;
                    isLeft = true;
                }
                else if (name == "CustomHandRight")
                {
                    // GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0, 0);
                    StartCoroutine("PoleControl");
                    isDo = true;
                    isLeft = false;
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


                GameObject obj = Instantiate(line);
                obj.GetComponent<KHG_Line>().SetLine(transform.Find("Line_Snap").position);


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

    IEnumerator SetBloodLineAlpha()
    {
        for (int i = 0; i < 6; i++)
        {
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.1f * i);
            yield return new WaitForSeconds(0.2f);
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
            BGB_Sap sap = GameObject.Find("Patient").GetComponent<BGB_Sap>();

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

}
