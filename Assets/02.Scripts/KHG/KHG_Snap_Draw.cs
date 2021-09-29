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

        Needle,
        Tourniquet,
        IVPole,
        Sap,
        Rubber,
    }


    private Material mat;
    private bool isDo = false;

    public ObjectType _objectType;

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


        if (_objectType == ObjectType.Tourniquet)
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

                // 다음 단계 시작 : 소독
                DrawingMgr.drawing.Disinfect();

                isDo = true;
            }
        }

    }

    Transform syringeBack;
    void Start()
    {
        if (_objectType == ObjectType.Pull)
        {
            syringeBack = GameObject.Find("Syringe_Back").transform;
        }
    }

    public void AirOffStart()
    {
        StartCoroutine("AirOff");
    }


    IEnumerator AirOff()
    {
        while (true)
        {
            if (_objectType == ObjectType.Pull)
            {
                if (transform.parent == null)
                {
                    transform.parent = syringeBack;

                    syringeBack.localPosition = Vector3.forward * -1.6f;
                    syringeBack.localEulerAngles = Vector3.zero;

                    transform.localPosition = new Vector3(0, 0.363f, 4f);
                    transform.localEulerAngles = Vector3.zero;

                }
                else if (transform.parent.parent.name == "CustomHandRight" || transform.parent.name == "CustomHandLeft")
                {
                    Transform temp = transform.parent;

                    transform.parent = syringeBack;

                    float dis = transform.localPosition.z;
                    if (dis > 4.0f)
                    {
                        syringeBack.localPosition += Vector3.forward * (dis - 4.0f) * 0.1f;
                    }

                    transform.parent = temp;

                }
                yield return new WaitForSeconds(0.1f);
            }

        }

    }
}
