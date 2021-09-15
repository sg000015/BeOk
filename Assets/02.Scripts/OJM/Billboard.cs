using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform camTr;

    void Start()
    {
        camTr = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.LookAt(camTr.position);
    }
}
