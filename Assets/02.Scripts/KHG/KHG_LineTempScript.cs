using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHG_LineTempScript : MonoBehaviour
{

    public KHG_Line kHG;
    // Start is called before the first frame update

    [ContextMenu("SetLine")]
    void Start()
    {
        kHG.SetLine(this.transform.position);
        ABCD();
    }


    [ContextMenu("SetColor")]
    void ABCD()
    {
        kHG.SetLineState(0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
