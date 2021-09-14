using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHG_Line : MonoBehaviour
{

    int status = -1;    // 넘버링 : 수액종류
    bool hasLine = false;

    LineRenderer line;

    public void SetLine(Vector3 pos)
    {

        line = GetComponent<LineRenderer>();
        line.positionCount = 125;
        Vector3 vec = Vector3.zero;
        Vector3 vec2 = pos;
        //! Find 이름 주의
        try { vec2 = pos + 0.05f * (GameObject.Find("Line_Snap").transform.position - GameObject.Find("Rubber").transform.position).normalized; }
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
        for (int i = 25; i < 45; i++)
        {
            vec = new Vector3(pos.x + (i - 25) * (-4.81f - pos.x) / 19f, pos.y + (i - 25) * (1.3f - pos.y) / 19f, -0.15f * Mathf.Sin(9f * (i - 25) / 57f) + pos.z + (i - 25) * (0.53f - pos.z) / 19f);
            line.SetPosition(i, vec);
        }
        pos = vec;
        for (int i = 45; i < 65; i++)
        {
            vec = new Vector3(pos.x + (i - 45) * (-4.88f - pos.x) / 19f, pos.y + (i - 45) * (1.26f - pos.y) / 19f, 0.15f * Mathf.Sin(4.5f * (i - 45) / 57f) + pos.z + (i - 45) * (0.60f - pos.z) / 19f);
            line.SetPosition(i, vec);
        }
        pos = vec;
        for (int i = 65; i < 85; i++)
        {
            vec = new Vector3(pos.x + (i - 65) * (-5.1f - pos.x) / 19f, pos.y + (i - 65) * (1.1f - pos.y) / 19f, 0.3f * Mathf.Sin(4.5f * (i - 65) / 57f) + pos.z + (i - 65) * (0.75f - pos.z) / 19f);
            line.SetPosition(i, vec);
        }
        pos = vec;
        for (int i = 85; i < 105; i++)
        {
            vec = new Vector3(pos.x + (i - 85) * (-5.33f - pos.x) / 19f, -0.5f * Mathf.Sin(9f * (i - 85) / 57f) + pos.y + (i - 85) * (0.92f - pos.y) / 19f, 0.15f * Mathf.Sin(4.5f * (i - 85) / 57f) + pos.z + (i - 85) * (1.25f - pos.z) / 19f);
            line.SetPosition(i, vec);
        }
        pos = vec;
        for (int i = 105; i < 125; i++)
        {
            vec = new Vector3(pos.x + (i - 105) * (-5.334f - pos.x) / 19f, pos.y + (i - 105) * (1.36f - pos.y) / 19f, pos.z + (i - 105) * (1.41f - pos.z) / 19f);
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

    IEnumerator SetLineColor()
    {
        line = GetComponent<LineRenderer>();
        WaitForSeconds ws = new WaitForSeconds(0.03f);

        Gradient gradient = new Gradient();
        gradient.mode = GradientMode.Fixed;

        // 알파값, 범위
        GradientAlphaKey[] gAlpha = new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f), new GradientAlphaKey(1.0f, 1.0f) };
        GradientColorKey[] gColor = new GradientColorKey[] { new GradientColorKey(Color.red, 1.0f), new GradientColorKey(Color.white, 1.0f) };

        // 정방향 ( 0: 상위칼라, 1: 하위칼라 , time : 범위 (0부터 n까지, Max=1))
        gColor[0].color = Color.red;
        gColor[1].color = Color.white;

        for (int i = 0; i < 100; i++)
        {
            gColor[0].time = 0.01f * i;
            gColor[1].time = 1.0f;
            gradient.SetKeys(gColor, gAlpha);
            line.colorGradient = gradient;
            yield return ws;
        }

        //역방향
        gColor[0].color = Color.red;
        gColor[1].color = Color.green;
        for (int i = 0; i < 100; i++)
        {
            gColor[0].time = 1.0f - 0.01f * i;
            gColor[1].time = 1.0f;
            gradient.SetKeys(gColor, gAlpha);
            line.colorGradient = gradient;
            yield return ws;
        }

    }
}
