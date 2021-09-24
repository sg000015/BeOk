using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioClip[] audios;
    public AudioClip[] patientAudioClips;
    // 0 : 아
    // 1 : 아파요
    // 2 : 아프다구요
    // 3 : 당신은 사람을 죽였어요
    // 4 : 최고예요

    AudioSource audio;
    public AudioSource patientAudio;

    void Start()
    {
        audio = GetComponent<AudioSource>();


    }

    public void Sound(int num)
    {
        audio.PlayOneShot(audios[num]);
    }
    public void SoundStop()
    {
        audio.Stop();
    }

    public void PlayPatientSound(int idx)
    {
        patientAudio.PlayOneShot(patientAudioClips[idx]);
    }


    [ContextMenu("a")]
    public void Sound2()
    {
        audio.PlayOneShot(audios[2]);
        Debug.Log("a");
    }




}
