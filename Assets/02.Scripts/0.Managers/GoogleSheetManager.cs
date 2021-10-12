using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class GoogleSheetManager : MonoBehaviour
{
    public bool isBlood = true;
    // Blood
    const string URLBlood = "https://docs.google.com/spreadsheets/d/1Gze08BVoxt-g9MQ982XxviePeaIDzPvJL5IS_6DnsK0/export?format=tsv";
    // Injection
    const string URLInjection = "https://docs.google.com/spreadsheets/d/1Gze08BVoxt-g9MQ982XxviePeaIDzPvJL5IS_6DnsK0/export?format=tsv&gid=54269727";


    public string[] UISentence;
    int lineSize, rowSize;

    IEnumerator Awake()
    {
        UnityWebRequest www;

        if (isBlood)
        {
            www = UnityWebRequest.Get(URLBlood);
        }
        else
        {
            www = UnityWebRequest.Get(URLInjection);
        }

        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;
        print(data);

        string[] line = data.Split('\n');
        lineSize = line.Length;
        rowSize = line[0].Split('\t').Length;
        UISentence = new string[lineSize];

        // 한 줄에서 탭으로 나눔
        for (int i = 0; i < lineSize; i++)
        {
            string[] row = line[i].Split('\t');
            UISentence[i] = row[0];
        }
        // 한 줄에서 탭으로 나눔
        for (int i = 0; i < lineSize; i++)
        {
            Debug.Log($"{UISentence[i]}");
        }
    }
}