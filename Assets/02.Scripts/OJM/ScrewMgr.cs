using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewMgr : MonoBehaviour
{
    private string whichHand = null;

    [SerializeField]
    private Transform hand_L;
    [SerializeField]
    private Transform hand_R;

    private float startAngle;
    private float currentAngle;

    private void Update()
    {
        // Check If There Is a Collision With "HAND"
        if (whichHand != null)
        {
            // 나사 돌리기

            currentAngle = (float)hand_L.transform.eulerAngles.x;
            Debug.Log($"rotation : {startAngle - currentAngle}");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Thumb_L"))
        {
            if (whichHand == null)
            {
                whichHand = "LEFT";
                startAngle = (float)hand_L.eulerAngles.x;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
    }
}
