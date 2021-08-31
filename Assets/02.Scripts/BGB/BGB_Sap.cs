using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGB_Sap : MonoBehaviour
{
    public Animator anim;
    public MeshRenderer handMesh;
    public SkinnedMeshRenderer patientMesh;
    float patientSpeed;
    string sapBag;

    float curSpeed;
    void Start()
    {
        patientSpeed = 10f;
        sapBag = "DW";
    }

    public void UpdateSpeed(int num)
    {
       curSpeed += num;

        Debug.Log(curSpeed);
       if (curSpeed > patientSpeed)
       {
           anim.SetBool("Trumble", true);
        Debug.Log("손떨림");
           
           return;
       }
       else if ( curSpeed < 0)
       {
           curSpeed = 0;
           return;
       }
       else
       {
           anim.SetBool("Trumble", false);

       }
    


       if(sapBag == "DW")
       {
           byte alphaPurple = (byte)Mathf.Round((curSpeed / patientSpeed * 255));
           if (alphaPurple > 20 ) alphaPurple = 20;
           
           handMesh.materials[3].color = new Color32(178, 0, 255, alphaPurple);
           patientMesh.materials[1].color = new Color32(255, 255, 255, alphaPurple);
       }
       else
       {
           byte alphaHives = (byte)Mathf.Round(255 - (curSpeed / patientSpeed * 255));
           handMesh.materials[1].color = new Color32(255,255,255, alphaHives);
       }
    }

}
