using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHG_Band : MonoBehaviour
{
    //! 이 스크립트는 테스트 플레이용이기에, 추후에 삭제될 것임을 알림
    public KHG_Line KHG;
    // Start is called before the first frame update
    void Start()
    {
        //Invoke("StartBand", 5.0f);
    }


    [ContextMenu("StartBand")]
    void StartBand()
    {
        StartCoroutine("SetBand");
    }


    IEnumerator SetBand()
    {


        Vector3 pos = new Vector3(-4.66f, 1.27f, 0.8f) - transform.position;
        Vector3 rot = new Vector3(0, 10, 0) - transform.localEulerAngles;
        rot.z = 0;

        Vector3 rot2 = transform.eulerAngles;


        for (int i = 0; i < 100; i++)
        {

            transform.position += pos * 0.01f;
            rot2 += rot * 0.01f;

            transform.rotation = Quaternion.Euler(rot2);

            KHG.SetLine();
            yield return new WaitForSeconds(0.02f);

        }

    }

}

