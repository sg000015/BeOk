using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AlcoholCottonMgr : MonoBehaviour
{
    private int rubCnt = 0;
    private int rubPointNum = 5;

    public Animator[] virusList;

    private void Start()
    {
        virusList = GameObject.Find("VirusGroup").GetComponentsInChildren<Animator>();
    }

    public void StartDisinfect()
    {
        StartCoroutine(nameof(ActiveVirus), rubCnt);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "VirusBody")
        {
            InactiveVirus(rubCnt);
            rubCnt++;
        }

        // Finish Disinfection
        if (rubCnt == rubPointNum)
        {
            // GameManager.instance.TestFinishDisinfect();
            return;
        }
        StartCoroutine(nameof(ActiveVirus), rubCnt);
    }

    IEnumerator ActiveVirus(int idx)
    {
        yield return new WaitForSeconds(1.0f);

        virusList[idx].GetComponentInChildren<Collider>().enabled = true;
        virusList[idx].GetComponent<Animator>().SetTrigger("isActive");
    }

    void InactiveVirus(int idx)
    {
        virusList[idx].GetComponentInChildren<Collider>().enabled = false;
        virusList[idx].GetComponent<Animator>().SetTrigger("isDie");
    }
}
