using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager_Lobby : MonoBehaviour
{
    private string userId;
    public TMP_Text userIdText;

    void Start()
    {
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
    }
}
