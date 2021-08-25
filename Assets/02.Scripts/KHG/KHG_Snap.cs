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

    }

    bool isDo = false;
    Material mat;

    public ObjectType _objectType = ObjectType.None;

    void OnTriggerEnter(Collider coll)
    {
        //Debug.Log(coll.name);

        if (_objectType == ObjectType.Needle)
        {
            if (!isDo && coll.gameObject.name == "Arm_Snap")
            {

                transform.parent.GetComponent<KHG_Needle>().NeedleSnap();
                isDo = true;
                Debug.Log("Arm to Snap");

            }
            else if (!isDo && coll.gameObject.name == "Fail_Snap")
            {

                Debug.Log("Fail to Snap");

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
                transform.localPosition = new Vector3(0.37f, -0.353f, -0.75f);
                transform.localEulerAngles = new Vector3(50f, -25f, 45f);

                mat = coll.transform.parent.GetComponent<MeshRenderer>().materials[1];  //숫자보정
                StartCoroutine("SetBloodLineAlpha");

                isDo = true;

            }
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
}
