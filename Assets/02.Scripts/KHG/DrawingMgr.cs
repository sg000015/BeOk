using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingMgr : MonoBehaviour
{

    public static DrawingMgr drawing;
    public GameObject patient;

    public GameObject Virusgroup;
    public GameObject Syringe;

    void Awake()
    {
        drawing = this;
    }

    [ContextMenu("1.지혈")]
    public void Hemostasis()
    {



        Virusgroup.SetActive(true);

    }


    [ContextMenu("2.소독")]
    public void Disinfect()
    {

        Syringe.transform.Find("Syringe_Back").Find("Pull_Snap").GetComponent<KHG_Snap_Draw>().AirOffStart();
    }

}
