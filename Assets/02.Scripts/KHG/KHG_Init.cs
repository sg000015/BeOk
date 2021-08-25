using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHG_Init : MonoBehaviour
{

    public GameObject Hand;
    // Start is called before the first frame update
    void Awake()
    {
        Hand.GetComponent<MeshRenderer>().materials[1].color = new Color(1f, 1f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
