using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public bool isMenu = false;
    public Timer timer;
    public GameObject ExitMenu;

    #region SINGLETON
    void Awake()
    {
        instance = this;
    }

    #endregion

    private void Start()
    {
    }

    private void Update()
    {
        // A 버튼 누름 && 메뉴 꺼져있음
        if (OVRInput.GetDown(OVRInput.Button.One) && !isMenu)
        {
            isMenu = true;

            // Timer Stop
            timer.timerOn = false;

            // UI On
            ExitMenu.SetActive(true);
        }

        // A 버튼 누름 && 메뉴 켜져있음
        if (OVRInput.GetDown(OVRInput.Button.One) && isMenu)
        {
            // Go to Lobby
            SceneManager.LoadScene("WatingRoom");
        }

        // B 버튼 누름 && 메뉴 켜져있음
        if (OVRInput.GetDown(OVRInput.Button.Two) && isMenu)
        {
            isMenu = false;

            // Timer Start
            timer.timerOn = true;

            // UI Off
            ExitMenu.SetActive(false);
        }
    }



}
