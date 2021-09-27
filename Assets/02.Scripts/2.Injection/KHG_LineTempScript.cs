using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHG_LineTempScript : MonoBehaviour
{
    //! 이 스크립트는 테스트 플레이용이기에, 추후에 삭제될 것임을 알림
    public KHG_Line kHG;
    public int _state;
    // Start is called before the first frame update



    [ContextMenu("SetLine")]
    void Start()
    {
        kHG.SetLine();
        SetColor();
    }


    [ContextMenu("SetColor")]
    void SetColor()
    {
        kHG.SetLineState(_state);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
