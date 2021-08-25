using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHG_CollTest : MonoBehaviour
{

    BoxCollider boxcoll;
    // Start is called before the first frame update
    void Start()
    {
        boxcoll = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Pos:" + boxcoll.center);
        Debug.Log("siz:" + boxcoll.size);

    }
}
