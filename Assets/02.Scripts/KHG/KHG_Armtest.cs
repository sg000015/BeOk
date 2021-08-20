using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHG_Armtest : MonoBehaviour
{

    public bool isShake;
    Vector3 vec;
    // Start is called before the first frame update
    void Start()
    {
        vec = transform.position;
    }

    private int i = 0;
    // Update is called once per frame
    void Update()
    {
        i++;
        if (i > 5)
        {
            i = 0;
            if (isShake)
            {
                transform.position = vec + Vector3.right * 0.001f * Random.Range(1, 5) + Vector3.up * 0.001f * Random.Range(1, 5);
            }

        }
    }
}
