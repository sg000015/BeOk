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


    public string[] Sentence_Injection;
    public string[] Sentence_Blood;
    int lineSize, rowSize;

    IEnumerator Start()
    {
        Debug.Log("GoogleSheetManager");
        UnityWebRequest www;
        www = UnityWebRequest.Get(URLInjection);
        yield return www.SendWebRequest();
        GetSentence(www, 0);


        www = UnityWebRequest.Get(URLBlood);
        yield return www.SendWebRequest();
        GetSentence(www, 1);
    }

    void GetSentence(UnityWebRequest www, int idx)
    {
        string data = www.downloadHandler.text;
        // print(data);

        string[] line = data.Split('\n');
        lineSize = line.Length;
        rowSize = line[0].Split('\t').Length;
        if (idx == 0)
        {
            Sentence_Injection = new string[lineSize];
        }
        else
        {
            Sentence_Blood = new string[lineSize];
        }

        for (int i = 0; i < lineSize; i++)
        {
            string[] row = line[i].Split('\t');
            string row2 = row[0].Replace("@", "\n");
            if (idx == 0)
            {
                Sentence_Injection[i] = row2;
            }
            else
            {
                Sentence_Blood[i] = row2;
            }
        }


        // Log

        // for (int i = 0; i < lineSize; i++)
        // {
        //     if (idx == 0)
        //     {
        //         Debug.Log($"{Sentence_Injection[i]}");
        //     }
        //     else
        //     {
        //         Debug.Log($"{Sentence_Blood[i]}");
        //     }
        // }
    }
}