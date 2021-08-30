using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGB_PatientCtrl : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

[ContextMenu("ActionAnim")]
    public void ActionAnim()
    {
        anim.SetBool("Dance", true);
    }

[ContextMenu("NoneActionAnim")]
    public void NoneActionAnim()
    {
        anim.SetBool("Dance", false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
