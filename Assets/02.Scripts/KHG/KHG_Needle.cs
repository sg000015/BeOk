using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHG_Needle : MonoBehaviour
{

    public Collider coll2;
    // Start is called before the first frame update
    void Start()
    {
        Collider coll3 = coll2;
        coll3.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision coll)
    {

        Debug.Log("OnCollisionEnter");
        Debug.Log(coll.gameObject.name);

        if (coll.gameObject.name == "syringe_1")
        {
            coll.transform.position = new Vector3(20.33691f, 0.3909f, -0.5628865f);
            coll.transform.eulerAngles = new Vector3(36.17f, -263.854f, -268.783f);
            coll.transform.localScale = Vector3.one * 0.9f;
            coll.gameObject.GetComponent<Rigidbody>().freezeRotation = true;

        }
    }

    void OnTriggerEnter(Collider coll)
    {


        Debug.Log("OnTriggerEnter");

    }

}
