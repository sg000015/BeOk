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

    private Collider zelcoColl;
    private Collider styletColl;
    private Collider needleColl;

    public GameObject needle;

    // Start is called before the first frame update
    void Start()
    {
        stylet = transform.Find("Stylet");
        styletColl = stylet.gameObject.GetComponent<BoxCollider>();
        needleColl = transform.Find("Needle").GetComponent<BoxCollider>();
        styletColl.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (isSnaped)
        {
            transform.position = pos;

            //찌르기 액션
            if (transform.parent == null && !isStabed) //&& zelcoColl.enabled
            {


                isStabed = true;
                pos = needle.transform.position;
                transform.position = pos;
                rot = transform.rotation;
                this.GetComponent<KHG_Grabble>().grabByState = KHG_Grabble.GrabByState.None;
                stylet.GetComponent<KHG_Grabble>().grabByState = KHG_Grabble.GrabByState.All;
                styletColl.enabled = true;
                //stylet.SetParent(null);
                //zelcoColl.enabled = false; 

                // gameObject.GetComponent<MeshRenderer>().material.color /= 1.2f;
                // styletColl.gameObject.GetComponent<MeshRenderer>().material.color /= 1.2f;
                Debug.Log("rot:" + rot.eulerAngles);

            }


            if (isStabed)
            {


                transform.localRotation = rot;

                if (stylet && Vector3.Distance(stylet.position, pos) > 0.5f)
                {
                    stylet.gameObject.GetComponent<KHG_Grabble>().destroy = true;

                }

            }
        }
    }

    public void NeedleSnap()
    {

        pos = transform.position;
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
