using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Intro : MonoBehaviour
{
    // Start is called before the first frame update
    public Animation anim;
    void Start()
    {
        StartCoroutine("SceneLoader");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SceneLoader()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Lobby");
    }

}
