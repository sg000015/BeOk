using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice;
using Photon.Voice.Unity;



public class CheckVoice : MonoBehaviour
{
    public Recorder recorder;
    public TMPro.TMP_Text text;

    void Start()
    {
        Invoke("Check",2f);
    }

    void Check()
    {
        text.text = recorder.MicrophoneDevice.ToString() + "\n";
        text.text += recorder.MicrophoneType.ToString() + "\n";
        text.text += Microphone.devices.ToString();

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
