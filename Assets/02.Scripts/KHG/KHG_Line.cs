using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHG_Line : MonoBehaviour
{

    int status = -1;    // 넘버링 : 수액종류
    bool hasLine = false;

    LineRenderer line;

    //함수 종류 : SetLine(), SetLineState(),StopLineState()





    public void SetLine()
    {
        //! Find 이름 주의
        Transform tr = GameObject.Find("Line_Snap").transform;
        Vector3 pos = tr.position;

        line = GetComponent<LineRenderer>();
        line.positionCount = 225;
        Vector3 vec = Vector3.zero;
        Vector3 vec2 = pos;
        try { vec2 = pos + 0.05f * (tr.position - tr.parent.position).normalized; }
        catch { }

        for (int i = 0; i < 5; i++)
        {
            line.SetPosition(i, pos);
        }
        for (int i = 5; i < 25; i++)
        {
            // vec = new Vector3(pos.x + (i - 5) * (vec2.x - 0.03f - pos.x) / 19f, pos.y + (i - 5) * (vec2.y + 0.02f - pos.y) / 19f, pos.z + (i - 5) * (vec2.z - 0.03f - pos.z) / 19f);
            vec = new Vector3(pos.x + (i - 5) * (vec2.x - pos.x) / 19f, pos.y + (i - 5) * (vec2.y - pos.y) / 19f, pos.z + (i - 5) * (vec2.z - pos.z) / 19f);
            line.SetPosition(i, vec);
        }
        pos = vec;
        for (int i = 25; i < 145; i++)
        {
            vec = new Vector3(pos.x + (i - 25) * (-4.81f - pos.x) / 119f, pos.y + (i - 25) * (1.3f - pos.y) / 119f, -0.15f * Mathf.Sin(1.5f * (i - 25) / 57f) + pos.z + (i - 25) * (0.53f - pos.z) / 119f);
            line.SetPosition(i, vec);
        }
        pos = vec;
        for (int i = 145; i < 165; i++)
        {
            vec = new Vector3(pos.x + (i - 145) * (-4.88f - pos.x) / 19f, pos.y + (i - 145) * (1.26f - pos.y) / 19f, 0.15f * Mathf.Sin(4.5f * (i - 145) / 57f) + pos.z + (i - 145) * (0.60f - pos.z) / 19f);
            line.SetPosition(i, vec);
        }
        pos = vec;
        for (int i = 165; i < 185; i++)
        {
            vec = new Vector3(pos.x + (i - 165) * (-5.1f - pos.x) / 19f, pos.y + (i - 165) * (1.1f - pos.y) / 19f, 0.3f * Mathf.Sin(4.5f * (i - 165) / 57f) + pos.z + (i - 165) * (0.75f - pos.z) / 19f);
            line.SetPosition(i, vec);
        }
        pos = vec;
        for (int i = 185; i < 205; i++)
        {
            vec = new Vector3(pos.x + (i - 185) * (-5.33f - pos.x) / 19f, -0.5f * Mathf.Sin(9f * (i - 185) / 57f) + pos.y + (i - 185) * (0.92f - pos.y) / 19f, 0.15f * Mathf.Sin(4.5f * (i - 185) / 57f) + pos.z + (i - 185) * (1.25f - pos.z) / 19f);
            line.SetPosition(i, vec);
        }
        pos = vec;
        for (int i = 205; i < 225; i++)
        {
            vec = new Vector3(pos.x + (i - 205) * (-5.334f - pos.x) / 19f, pos.y + (i - 205) * (1.36f - pos.y) / 19f, pos.z + (i - 205) * (1.41f - pos.z) / 19f);
            line.SetPosition(i, vec);
        }

        //보정?
        line.SetPosition(6, line.GetPosition(5));
        line.SetPosition(86, line.GetPosition(85));

        hasLine = true;


    }



    public void SetLineState(int _status)
    {
        if (!hasLine) { Debug.Log("There is no Line"); return; }

        //수액 종류 설정
        status = _status;

        StartCoroutine("SetLineColor");
    }

    public void StopLineState()
    {
        status = -1;
        StopCoroutine("SetLineColor");
    }



    IEnumerator SetLineColor()
    {
        line = GetComponent<LineRenderer>();
        WaitForSeconds ws = new WaitForSeconds(0.04f);

        Gradient gradient = new Gradient();
        gradient.mode = GradientMode.Fixed;

        // 알파값, 범위
        GradientAlphaKey[] gAlpha = new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f), new GradientAlphaKey(1.0f, 1.0f) };
        GradientColorKey[] gColor = new GradientColorKey[] { new GradientColorKey(Color.red, 1.0f), new GradientColorKey(Color.white, 1.0f) };


        // // 정방향 ( 0: 상위칼라, 1: 하위칼라 , time : 범위 (0부터 n까지, Max=1))
        // gColor[0].color = Color.red;
        // gColor[1].color = Color.white;

        // for (int i = 0; i < 100; i++)
        // {
        //     gColor[0].time = 0.01f * i;
        //     gColor[1].time = 1.0f;
        //     gradient.SetKeys(gColor, gAlpha);
        //     line.colorGradient = gradient;
        //     yield return ws;
        // }

        // //역방향
        // gColor[0].color = Color.red;
        // gColor[1].color = Color.green;
        // for (int i = 0; i < 100; i++)
        // {
        //     gColor[0].time = 1.0f - 0.01f * i;
        //     gColor[1].time = 1.0f;
        //     gradient.SetKeys(gColor, gAlpha);
        //     line.colorGradient = gradient;
        //     yield return ws;
        // }


        switch (status)
        {
            case 0:
                gColor[0].color = Color.red;
                gColor[1].color = Color.white;
                gAlpha[0].alpha = 0.8f;
                gAlpha[0].time = 0.1f;
                gradient.mode = GradientMode.Blend;
                break;
            case 1:
                gColor[0].color = Color.red;
                gColor[1].color = Color.white;
                gAlpha[0].alpha = 0.8f;
                gAlpha[0].time = 0.1f;
                gradient.mode = GradientMode.Blend;
                break;
            case 2:
                gColor[0].color = Color.white;
                gColor[1].color = Color.green;
                gAlpha[0].alpha = 1.0f;
                gradient.mode = GradientMode.Fixed;
                break;
            case 3:
                gColor[0].color = Color.white;
                gColor[1].color = Color.blue;
                gAlpha[0].alpha = 1.0f;
                gradient.mode = GradientMode.Fixed;
                break;
            default:
                gColor[0].color = Color.white;
                gColor[1].color = Color.white;
                gAlpha[0].alpha = 1.0f;
                gradient.mode = GradientMode.Fixed;
                break;

        }


        for (int i = 0; i < 100; i++)
        {
            //status 0,1은 혈액, 나머지는 수액
            if (status == 0) { gColor[0].time = 0.0005f * i; gColor[1].time = 0.001f * i; }
            else if (status == 1) { gColor[0].time = 0.05f - 0.0005f * i; gColor[1].time = 0.1f - 0.001f * i; }
            else { gColor[0].time = 1f - 0.01f * (i + 1); gColor[1].time = 1.0f; }
            gradient.SetKeys(gColor, gAlpha);
            line.colorGradient = gradient;
            yield return ws;
            if (status == -1) { StopCoroutine("SetLineColor"); }
        }

    }
}
