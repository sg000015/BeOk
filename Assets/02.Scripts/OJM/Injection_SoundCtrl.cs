using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Injection_SoundCtrl : MonoBehaviour
{
    private new AudioSource audio;
    public AudioClip[] audioClips;
    // 0 : 아
    // 1 : 아파요
    // 2 : 아프다구요
    // 3 : 당신은 사람을 죽였어요
    // 4 : 최고예요

    void Start()
    {
        audio = this.GetComponent<AudioSource>();
    }

    public void PlayPatientSound(int idx)
    {
        audio.PlayOneShot(audioClips[idx]);
    }
}
