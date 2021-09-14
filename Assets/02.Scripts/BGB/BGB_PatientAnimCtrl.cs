using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGB_PatientAnimCtrl : MonoBehaviour
{
    public Animator anim;

    [ContextMenu("ToIdle")]
    public void ToIdle()
    {
        anim.SetTrigger("Idle");
    }

    [ContextMenu("IdleToInjection")]

    public void IdleToInjection()
    {
        anim.SetTrigger("IdleToInjection");
    }

    [ContextMenu("ToTremble")]
    public void ToTremble()
    {
        anim.SetTrigger("Tremble");
    }

    [ContextMenu("HeadShaking")]
    public void ToHeadShaking()
    {
        anim.SetTrigger("HeadShaking");
    }

    [ContextMenu("BedCrush")]
    public void ToBedCrush()
    {
        anim.SetTrigger("BedCrush");
    }

    [ContextMenu("WrongAngle")]
    public void WrongAngle()
    {
        anim.SetTrigger("WrongAngle");
    }
}
