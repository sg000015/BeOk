using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHG_Needle : MonoBehaviour
{

    public bool isSnaped = false;
    public bool isStabed = false;


    public Vector3 pos = Vector3.zero;
    public Quaternion rot = Quaternion.identity;


    private Transform stylet;

    private Collider zelcoColl;
    private Collider styletColl;

    public GameObject needle;

    // Start is called before the first frame update
    void Start()
    {
        stylet = transform.Find("Stylet");
        styletColl = stylet.gameObject.GetComponent<BoxCollider>();
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
                rot = transform.rotation;

                stylet.GetComponent<KHG_Grabble>().grabByState = KHG_Grabble.GrabByState.All;
                styletColl.enabled = true;
                stylet.SetParent(null);
                //zelcoColl.enabled = false; 

                // gameObject.GetComponent<MeshRenderer>().material.color /= 1.2f;
                // styletColl.gameObject.GetComponent<MeshRenderer>().material.color /= 1.2f;

            }


            if (isStabed)
            {
                transform.localRotation = rot;
                if (Vector3.Distance(stylet.position, pos) > 0.5f)
                {
                    stylet.gameObject.SetActive(false);
                    gameObject.GetComponent<KHG_Grabble>().grabByState = KHG_Grabble.GrabByState.None;

                }

            }
        }
    }


    void OnTriggerEnter(Collider coll)
    {

        if (!isSnaped && coll.gameObject.name == "Arm_Snap")
        {
            Debug.Log("OnTriggerEnter");


            //gameObject.GetComponent<KHG_Grabble>().grabByState = KHG_Grabble.GrabByState.None;
            pos = transform.position;
            // gameObject.GetComponent<MeshRenderer>().material.color *= 1.2f;
            // styletColl.gameObject.GetComponent<MeshRenderer>().material.color *= 1.2f;

            isSnaped = true;

        }
    }

}
