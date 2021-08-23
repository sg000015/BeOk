using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGB_Sap : MonoBehaviour
{
    public Animator anim;
    public MeshRenderer handMesh;
    float patientSpeed;
    float curSpeed;
    // Start is called before the first frame update
    void Start()
    {
        patientSpeed = 10f;
    }

    public void UpdateSpeed(int num)
    {
     curSpeed += num;
       if (curSpeed > patientSpeed)
       {
           Debug.Log("손떨림");
           anim.SetBool("HandShaking", true);
           return;
       }
       else if ( curSpeed < 0)
       {
           curSpeed = 0;
           return;
       }
       else
       {
           anim.SetBool("HandShaking", false);

       }
        
       byte alpha = (byte)Mathf.Round(255 - (curSpeed / patientSpeed * 255));

       handMesh.materials[1].color = new Color32(255,255,255, alpha);
    }

}
