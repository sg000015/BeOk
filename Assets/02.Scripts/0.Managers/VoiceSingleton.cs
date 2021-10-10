using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class VoiceSingleton : MonoBehaviourPunCallbacks
{
    // public static VoiceSingleton instanceVoice= null;
    // void Awake()
    // {
    //     if (instanceVoice == null)
    //     {
    //         instanceVoice = this;
    //         DontDestroyOnLoad(this.gameObject);
    //         Debug.Log("싱글톤 초기화");
    //     }
    //     else
    //     {
    //         Destroy(this.gameObject);
    //         Debug.Log("싱글톤 존재해서 삭제");
    //     }

    // }

    public override void OnLeftRoom()
    {
        Destroy(this.gameObject);
    }
}
