using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHG_AudioClip : MonoBehaviour
{

    public AudioClip[] audios;

    AudioSource audio;

    public void Sound(int num)
    {
        audio.PlayOneShot(audios[num]);
    }




}
