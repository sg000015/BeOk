using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class GameManager_Lobby : MonoBehaviour
{
    private string userId;
    public TMP_Text userIdText;
    private FirebaseManager fb;

    public TMP_Text[] names;
    public TMP_Text[] scores;
    public TMP_Text[] times;

    void Start()
    {
        fb = this.GetComponent<FirebaseManager>();
        fb.rankingEvent.AddListener(() => UpdateRanking());

        userId = PlayerPrefs.GetString("USER_ID", $"USER_{Random.Range(0, 100):00}");
        userIdText.text = userId;
        SetNickname();
    }

    void Update()
    {

    }

    public void SetNickname()
    {
        PlayerPrefs.SetString("USER_ID", userIdText.text);
        fb.SetNickname();
    }

    public void GetRanking(string skill)
    {
        // 본인 점수 가져오기
        fb.SelectData(skill);

        // 랭커들 점수 가져오기
        fb.LoadAllData(skill);
    }

    public void UpdateRanking()
    {
        int rankNum = fb.rankNum;

        names[0].text = fb.rankersName[rankNum];
        scores[0].text = $"{fb.rankersScore[rankNum]}";
        times[0].text = $"{fb.rankersMin[rankNum]}:{fb.rankersSec[rankNum]}";

        for (int i = 0; i < rankNum; i++)
        {
            names[i + 1].text = fb.rankersName[i];
            scores[i + 1].text = $"{fb.rankersScore[i]}";
            times[i + 1].text = $"{fb.rankersMin[i]}:{fb.rankersSec[i]}";
        }

    }
}
