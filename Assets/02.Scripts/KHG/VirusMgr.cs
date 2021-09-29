using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusMgr : MonoBehaviour
{
    private int rubCnt = 0;
    private int rubPointNum = 5;

    public GameObject virusGroupObj;
    public Animator[] virusList;

    public GameObject virusFx;
    public GameObject disinfectFx;
    public SoundManager soundManager;

    private void Start()
    {
        StartDisinfect();
    }

    // private void SecondDisinfect()
    // {
    //     virusList[1].SetTrigger("isActive");
    //     virusList[2].SetTrigger("isActive");
    //     virusList[3].SetTrigger("isActive");
    //     virusList[4].SetTrigger("isActive");

    // }

    public void StartDisinfect()
    {
        StartCoroutine(nameof(ActiveVirus), 0);
    }

    public void AlcoholCottonEnter()
    {
        StartCoroutine(nameof(InactiveVirus), rubCnt);
        rubCnt++;
        // Debug.Log($"rubCnt(Virus) : {rubCnt}");

        if (rubCnt == 1)
        {
            StartCoroutine(nameof(ActiveVirus), 1);
            StartCoroutine(nameof(ActiveVirus), 2);
            StartCoroutine(nameof(ActiveVirus), 3);
            StartCoroutine(nameof(ActiveVirus), 4);
        }

        // Finish Disinfection
        if (rubCnt == rubPointNum)
        {
            StartCoroutine(nameof(FinishDisinfect));
            return;
        }

    }

    IEnumerator ActiveVirus(int idx)
    {
        yield return new WaitForSeconds(1.0f);

        //! 여기서 에러 발생
        virusList[idx].GetComponentInChildren<Collider>().enabled = true;
        virusList[idx].GetComponent<Animator>().SetTrigger("isActive");
    }

    IEnumerator InactiveVirus(int idx)
    {
        virusList[idx].GetComponentInChildren<Collider>().enabled = false;
        virusList[idx].GetComponent<Animator>().SetTrigger("isDie");
        GameObject virusFxObj = Instantiate(virusFx, virusList[idx].transform.position, Quaternion.identity);

        soundManager.Sound(0);

        Destroy(virusFxObj, 2.0f);
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
        GameObject disinfectFxObj = Instantiate(disinfectFx, pos, Quaternion.identity);
        soundManager.Sound(4);


        yield return new WaitForSeconds(3);
        Destroy(disinfectFxObj);

        // 바이러스그룹 비활성화
        virusGroupObj.SetActive(false);

        // 다음 단계 시작 : 주사 위치
        DrawingMgr.drawing.Disinfect();
    }
}
