using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("AAA");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator AAA()
    {
        WaitForSeconds ws = new WaitForSeconds(0.2f);
        int a = 0;
        while (true)
        {
            for (int i = 0; i < 5; i++)
            {
                Debug.Log("+++");
                yield return ws;
            }
            for (int i = 0; i < 5; i++)
            {
                Debug.Log("---");
                yield return ws;
            }
            a++;

            if (a > 2)
            {
                StopCoroutine("AAA");
                break;
            }

        }


    }
}
