using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHG_Scernario : MonoBehaviour
{


    public enum Scernario_state
    {
        intro,
        Lobby,
        Ready,
    }

    public List<GameObject> StageList;
    public Dictionary<Scernario_state, GameObject> StageDictionary = new Dictionary<Scernario_state, GameObject>();




    private Scernario_state scernario_State;

    void Awake()
    {
        scernario_State = Scernario_state.intro;

        SetStageDictionary();
    }

    void SetStageDictionary()
    {
        StageDictionary.Add(Scernario_state.intro, StageList[0]);
        StageDictionary.Add(Scernario_state.Lobby, StageList[1]);
        StageDictionary.Add(Scernario_state.Ready, StageList[2]);
    }


    void Start()
    {
        StartCoroutine("StartIntro");
    }


    void Update()
    {

    }

    IEnumerator StartIntro()
    {
        Debug.Log("(대충 인트로1)");
        yield return new WaitForSeconds(1.0f);
        Debug.Log("(대충 인트로2)");
        yield return new WaitForSeconds(1.0f);
        Debug.Log("(대충 인트로3)");
        yield return new WaitForSeconds(1.0f);

        ChangeScernario(Scernario_state.Lobby);
        Debug.Log("시작");
    }


    public void ChangeScernario(Scernario_state state)
    {
        this.scernario_State = state;
        if (state == Scernario_state.Lobby)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position += new Vector3(100f, 0.4f, 0);

        }
    }


}
