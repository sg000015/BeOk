using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NusingSkills
{ }

public class A_Skill
{
    public string drug;
    public float amountDrug;
    public string[] needs;

    // 생성자
    A_Skill(float amountDrug)
    {
        this.amountDrug = amountDrug;
    }

    // 평가하는 함수
    // ...
}

public class B_Skill
{
    public string drug;
    public float amountDrug;
    public string[] needs;

    // 생성자
    B_Skill(float amountDrug)
    {
        this.amountDrug = amountDrug;
    }

    // 평가하는 함수
    // ...
}
