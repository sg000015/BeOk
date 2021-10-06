using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VesselColor : MonoBehaviour
{

    public bool check = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("HightedColor");
    }

    IEnumerator HightedColor()
    {
        Material _mat = GetComponent<MeshRenderer>().material;
        BoxCollider box = GetComponent<BoxCollider>();
        WaitForSeconds ws = new WaitForSeconds(0.03f);
        Color _color = _mat.color;
        while (true)
        {
            if (box.enabled)
            {
                for (int i = 0; i < 5; i++)
                {
                    _mat.color = new Color(0.8f, 0.8f - 0.2f * i, 0.8f - 0.1f * i);
                    yield return ws;
                }
                for (int i = 1; i < 5; i++)
                {
                    _mat.color = new Color(0.8f, 0.2f * i, 0.4f + 0.1f * i);
                    yield return ws;
                }
            }
            yield return ws;
            //!조건
            if (check)
            {
                _mat.color = _color;
                StopCoroutine("HightedColor");
                break;
            }
        }
    }

}
