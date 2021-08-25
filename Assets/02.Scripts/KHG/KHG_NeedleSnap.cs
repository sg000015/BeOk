using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHG_NeedleSnap : MonoBehaviour
{

    bool a = false;

    void OnTriggerEnter(Collider coll)
    {

        if (!a && coll.gameObject.name == "Arm_Snap")
        {

            transform.parent.GetComponent<KHG_Needle>().NeedleSnap();
            a = true;

        }
    }
}
