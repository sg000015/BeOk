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
    public bool isInjection = true;

    void Start()
    {
    }

    public void Sound(int num)
    {
        //1013
        if (num == 1)
        {
            StartCoroutine(nameof(Haptic), 0.5f);
        }
        audio.PlayOneShot(audios[num]);
    }
    public void SoundStop()
    {
        audio.Stop();
    }

    public void PlayPatientSound(int idx)
    {
        if (isInjection && (idx <= 2 || idx <= 6))
        {
            StartCoroutine(nameof(Haptic), 0.5f);
        }
        else if (!isInjection && idx == 0)
        {

        }
        patientAudio.PlayOneShot(patientAudioClips[idx]);
    }


    [ContextMenu("a")]
    public void Sound2()
    {
        audio.PlayOneShot(audios[2]);
        Debug.Log("a");
    }

    IEnumerator Haptic(float duration)
    {
        // 주파수, 진폭, 위치
        OVRInput.SetControllerVibration(0.8f, 0.8f, OVRInput.Controller.RTouch);
        OVRInput.SetControllerVibration(0.8f, 0.8f, OVRInput.Controller.LTouch);
        yield return new WaitForSeconds(duration);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);

    }




}
