using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGB_CCTV_Injection : MonoBehaviour
{
    public GameObject CCTV_1;
    public GameObject CCTV_2;
    public GameObject CCTV_3;

    GameObject prevCCTV;
    GameObject CCTVvr;
    void Start()
    {
        prevCCTV = CCTV_1;
    }
    // Start is called before the first frame update
    public void CCTVChange(GameObject curCCTV)
    {
        Debug.Log("Aaa");
        prevCCTV.SetActive(false);
        curCCTV.SetActive(true);

        prevCCTV = curCCTV;
    }

    public void CCTVVR()
    {
        prevCCTV.SetActive(false);
        CCTVvr = GameObject.Find("Player(Clone)")?.transform.GetChild(0).Find("CenterEyeAnchor").gameObject; 
                 CCTVvr.SetActive(true);

        prevCCTV = CCTVvr;

    }
}
