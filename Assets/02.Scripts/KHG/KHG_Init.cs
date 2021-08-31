using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHG_Init : MonoBehaviour
{

    public GameObject Hand;
    public GameObject newNeedle;
    // Start is called before the first frame update
    void Awake()
    {
        Hand.GetComponent<MeshRenderer>().materials[2].color = new Color(1f, 1f, 1f, 0f); //!숫자보정
        Instantiate(newNeedle);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NewNeedle()
    {
        Instantiate(newNeedle);
    }
}
