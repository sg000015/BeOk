using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceDetected : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetString("Device", "Phone");
    }
}
