using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaccumTubeMgr : MonoBehaviour
{

    public GameObject[] vials;
    public int a;
    Material mat;
    // Start is called before the first frame update
    void Start()
    {
        mat = transform.GetChild(0).GetComponent<MeshRenderer>().material;
        SetBloodType();
    }

    void SetBloodType()
    {
        a = Random.Range(a, a + 3);
        switch (a)
        {
            case 0:
                mat.color = new Color(0, 0.8f, 1);
                vials[0].SetActive(false);
                break;
            case 1:
                mat.color = Color.red;
                vials[1].SetActive(false);
                break;
            case 2:
                mat.color = Color.yellow;
                vials[2].SetActive(false);
                break;
            case 3:
                mat.color = Color.green;
                vials[0].SetActive(false);
                break;
            case 4:
                mat.color = new Color(0.8f, 0, 0.8f);
                vials[1].SetActive(false);
                break;
            case 5:
                mat.color = Color.gray;
                vials[2].SetActive(false);
                break;

            default:
                mat.color = Color.white;
                break;

        }



    }
}
