using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private float h;
    private float v;
    private float moveSpeed = 1.0f;
    private float turnSpeed = 200.0f;

    void Start()
    {
    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        transform.Translate(moveDir.normalized * Time.deltaTime * moveSpeed, Space.Self);
        // transform.Rotate(Vector3.up * Time.deltaTime * turnSpeed * h);

    }
}
