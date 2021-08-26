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
    public GameObject disinfectFx;

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
            StartCoroutine(nameof(InactiveVirus), rubCnt);
            rubCnt++;
        }

        // Finish Disinfection
        if (rubCnt == rubPointNum)
        {
            StartCoroutine(nameof(FinishDisinfect));
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

        Destroy(tempFx, 2.0f);
        yield return new WaitForSeconds(.7f);
        while (true)
        {
            yield return new WaitForFixedUpdate();
            virusList[idx].transform.localScale -= virusList[idx].transform.localScale * 0.05f;
            if (virusList[idx].transform.localScale.x <= 0)
            {
                Debug.Log("here");
                break;
            }
        }
    }

    IEnumerator FinishDisinfect()
    {
        yield return new WaitForSeconds(1.3f);
        Vector3 pos = GameObject.Find("DisinfectFxPivot").GetComponent<Transform>().position;
        GameObject tempFx2 = Instantiate(disinfectFx, pos, Quaternion.identity);

        yield return new WaitForSeconds(3);
        Destroy(tempFx2);

        // TODO
        // GameManager.instance.TestFinishDisinfect();

    }
}
