using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHG_Band : MonoBehaviour
{

    public KHG_Line KHG;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("ABC", 5.0f);
    }


    [ContextMenu("StartBand")]
    void ABC()
    {
        StartCoroutine("ABCD");
    }


    IEnumerator ABCD()
    {


        Vector3 pos = new Vector3(-4.66f, 1.27f, 0.8f) - transform.position;
        Vector3 rot = new Vector3(0, 10, 0) - transform.localEulerAngles;
        rot.z = 0;
        Debug.Log(transform.localEulerAngles);
        Debug.Log(transform.localEulerAngles.x);
        Debug.Log(transform.localEulerAngles.y);
        Debug.Log(transform.localEulerAngles.z);
        Debug.Log(rot * 0.01f);

        Transform lineSnap = transform.Find("Line_Snap");

        Vector3 rot2 = transform.eulerAngles;


        for (int i = 0; i < 100; i++)
        {

            transform.position += pos * 0.01f;
            rot2 += rot * 0.01f;

            transform.rotation = Quaternion.Euler(rot2);



            KHG.SetLine(lineSnap.position);
            yield return new WaitForSeconds(0.02f);

        }

    }

}

