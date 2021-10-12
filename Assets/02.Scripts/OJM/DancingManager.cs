using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DancingManager : MonoBehaviour
{
    int idxNum = 1;
    float time = 8.0f;
    float speed = 600.0f;
    Vector3 originPos = new Vector3(-6, 0, 0);
    Vector3[] posList = { new Vector3(2052, 0, 0), new Vector3(-2052, 0, 0) };
    bool isLeft = true;

    public Transform David;
    // public Transform image;
    public RectTransform image;
    private Animator anim;

    void Start()
    {
        anim = David.GetComponent<Animator>();
        StartCoroutine(nameof(Dance));
    }

    void Update()
    {
        if (isLeft)
        {
            image.position = Vector3.MoveTowards(image.position, image.position + Vector3.right * 100, speed * Time.deltaTime);
        }
        else
        {
            image.position = Vector3.MoveTowards(image.position, image.position + Vector3.right * -100, speed * Time.deltaTime);
        }
    }

    IEnumerator Dance()
    {
        while (true)
        {
            yield return new WaitForSeconds(time);

            David.position = originPos;
            David.eulerAngles = Vector3.zero;

            idxNum++;
            if (idxNum == 4) idxNum = 1;

            anim.SetInteger("Index", idxNum);
            isLeft = !isLeft;
        }

    }
}
