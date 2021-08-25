using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AlcoholCottonMgr : MonoBehaviour
{
    private int rubCnt = 0;
    private int rubPointNum = 5;

    public Animator[] virusList;

    public GameObject virusFx;
    public GameObject DisinfectFx;

    private void Start()
    {
        virusList = GameObject.Find("VirusGroup").GetComponentsInChildren<Animator>();
    }

    public void StartDisinfect()
    {
        Debug.Log("StartDisinfect");
        StartCoroutine(nameof(ActiveVirus), rubCnt);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "VirusBody")
        {
            StartCoroutine(nameof(InactiveVirus), rubCnt);
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

    IEnumerator InactiveVirus(int idx)
    {
        virusList[idx].GetComponentInChildren<Collider>().enabled = false;
        virusList[idx].GetComponent<Animator>().SetTrigger("isDie");
        GameObject tempFx = Instantiate(virusFx, virusList[idx].transform.position, Quaternion.identity);
        yield return new WaitForSeconds(.7f);
        // 파티클 생성
        while (true)
        {
            yield return new WaitForFixedUpdate();
            virusList[idx].transform.localScale -= virusList[idx].transform.localScale * 0.05f;
            if (virusList[idx].transform.localScale.x <= 0)
            {
                break;
            }
        }
        // 파티클 제거
        Destroy(tempFx);
    }
}
