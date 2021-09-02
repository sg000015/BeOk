using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHG_Line : MonoBehaviour
{


    LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        SetLine(new Vector3(-4.7f, 1.25f, 0.61f));
    }


    public void SetLine(Vector3 pos)
    {
        line.positionCount = 230;
        Vector3 vec = Vector3.zero;
        for (int i = 0; i < 45; i++)
        {
            vec = new Vector3(pos.x + (-5.0f - pos.x) / 90 * i, 0.05f * Mathf.Sin(i / 14f) + pos.y + (1.2f - pos.y) / 90 * i, pos.z + (0.8f - pos.z) / 90 * i);
            line.SetPosition(i, vec);
        }
        for (int i = 45; i < 90; i++)
        {
            vec = new Vector3(pos.x + (-5.0f - pos.x) / 90 * i, 0.1f * Mathf.Sin(i / 14f) + pos.y + (1.2f - pos.y) / 90 * i, pos.z + (0.8f - pos.z) / 90 * i);
            line.SetPosition(i, vec);
        }
        pos = vec;
        for (int i = 0; i < 45; i++)
        {
            vec = new Vector3(0.05f * Mathf.Sin(i / 14f) + pos.x + (-5.25f - pos.x) / 90 * i, 0.1f * Mathf.Sin(i / 14f) + pos.y + (0f - pos.y) / 90 * i, pos.z + 0.1f * Mathf.Sin(i / 14f) + (1.2f - pos.z) / 90 * i);
            line.SetPosition(i + 90, vec);
        }
        for (int i = 45; i < 90; i++)
        {
            vec = new Vector3(0.05f * Mathf.Sin(i / 14f) + pos.x + (-5.25f - pos.x) / 90 * i, 0.05f * Mathf.Sin(i / 14f) + pos.y + (0f - pos.y) / 90 * i, pos.z + 0.1f * Mathf.Sin(i / 14f) + (1.2f - pos.z) / 90 * i);
            line.SetPosition(i + 90, vec);
        }
        pos = vec;
        for (int i = 0; i < 45; i++)
        {
            vec = new Vector3(0.05f * Mathf.Sin(i / 14f) + pos.x + (-5.30f - pos.x) / 90 * i, 0.1f * Mathf.Sin(i / 14f) + pos.y + (0.12f - pos.y) / 90 * i, pos.z + 0.1f * Mathf.Sin(i / 14f) + (1.36f - pos.z) / 90 * i);
            line.SetPosition(i + 180, vec);
        }
        for (int i = 45; i < 50; i++)
        {
            vec = new Vector3(-5.334f, pos.y + (1.36f - pos.y) / (50 - i), 1.41f);
            line.SetPosition(i + 180, vec);
        }

    }
}
