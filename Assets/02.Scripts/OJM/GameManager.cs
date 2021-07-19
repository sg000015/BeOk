using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    // Events
    public DefaultEvent testEvent;

    #region SINGLETON
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    #endregion

    private void Start()
    {
        testEvent.AddListener(() => TestFun());
    }

    // 소독
    public void Disinfect()
    {

    }

    // 지혈
    public void Hemostasis()
    {

    }

    #region TEST_FUNCTION

    public void TestFinishDisinfect()
    {
        testEvent.Invoke();
    }

    private void TestFun()
    {
        Debug.Log("Test About Event");
    }

    #endregion


}

#region UNITY_EVENT
[System.Serializable]
public class DefaultEvent : UnityEvent
{
}

#endregion

/*
토니켓묶기
알콜솜 바르기
주사하기
지혈하기
수액의 양 조절하기
*/
