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


    }

    public void Sound(int num)
    {
        audio.PlayOneShot(audios[num]);
    }


    [ContextMenu("a")]
    public void Sound2()
    {
        audio.PlayOneShot(audios[2]);
        Debug.Log("a");
    }




}
