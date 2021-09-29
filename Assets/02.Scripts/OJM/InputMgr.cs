using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMgr : MonoBehaviour
{
    public FirebaseManager fb;

    public string nickname = "temp00";
    public int score = 10;
    public int minute = 1;
    public int second = 10;

    void Start()
    {

    }

    [ContextMenu("정맥주사 - 삽입")]
    void InjectionInsert()
    {
        int _score = score * 10000;
        int min = 60 - minute;
        int sec = 60 - second;

        _score += min * 100;
        _score += sec;
        fb.InsertData("Injection", nickname, _score);
    }

    [ContextMenu("정맥주사 - All")]
    void InjectionLoadAllData()
    {
        fb.LoadAllData("Injection");
    }

    [ContextMenu("정맥주사 - 내 점수")]
    void InjectionSelect()
    {
        fb.SelectData("Injection", nickname);
    }
}
