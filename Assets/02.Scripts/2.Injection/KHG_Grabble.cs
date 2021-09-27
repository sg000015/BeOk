using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHG_Grabble : MonoBehaviour
{

    public enum GrabByState { All, Pinch, Grab, None }



    public GrabByState grabByState = GrabByState.All;

    public bool isExit = false;
    public bool destroy = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (destroy)
        {
            Destroy(this.gameObject);
        }
    }
}
