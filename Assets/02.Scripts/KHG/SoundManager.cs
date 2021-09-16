using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioClip[] audios;

    AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.Play();
    }

    public void Sound(int num)
    {
        audio.PlayOneShot(audios[num]);
    }
    public void SoundStop()
    {
        audio.Stop();
    }




}
