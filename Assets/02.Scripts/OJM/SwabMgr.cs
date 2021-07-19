using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SwabMgr : MonoBehaviour
{
    private int rubCnt = 0;
    private int rubPointNum = 9;

    private void OnTriggerEnter(Collider other)
    {
        string collName = other.gameObject.name;
        string[] collNameList = collName.Split('_');

        if (collNameList[0] == "RubPoint")
        {
            rubCnt++;
            Destroy(other.gameObject);
        }

        // Finish Disinfection
        if (rubCnt == rubPointNum)
        {
            GameManager.instance.TestFinishDisinfect();
        }
    }
}
