using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioClip[] audios;
    public AudioClip[] patientAudioClips;
    /*
    0 : 아
    1 : 아파요
    2 : 아프다구요
    3 : 당신은 사람을 죽였어요
    4 : 최고예요
    5 : 나쁘지 않아요
    6 : 똑바로 하세요

    0 : 으
    1 : 최악
    2 : 나쁘진않네요
    3 : 최고
    */

    public AudioSource audio;
    public AudioSource patientAudio;

    void Start()
    {


    }

    public void Sound(int num)
    {
        //1013
        if (num == 1)
        {
            OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);
        }
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
