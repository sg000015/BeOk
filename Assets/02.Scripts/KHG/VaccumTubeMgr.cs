using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class VaccumTubeMgr : MonoBehaviourPunCallbacks
{

    public GameObject[] vials;
    public int a = -1;
    Material mat;
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            a = Random.Range(a, a + 3);
            SetBloodType(a);
        }
        mat = transform.GetChild(0).GetComponent<MeshRenderer>().material;

    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
            gameObject.GetPhotonView().RPC(nameof(SetBloodType), RpcTarget.Others, a);
    }

    [PunRPC]
    void SetBloodType(int a)
    {
        switch (a)
        {
            case 0:
                mat.color = new Color(0, 0.8f, 1);
                vials[0].SetActive(false);
                break;
            case 1:
                mat.color = Color.red;
                vials[1].SetActive(false);
                break;
            case 2:
                mat.color = Color.yellow;
                vials[2].SetActive(false);
                break;
            case 3:
                mat.color = Color.green;
                vials[0].SetActive(false);
                break;
            case 4:
                mat.color = new Color(0.8f, 0, 0.8f);
                vials[1].SetActive(false);
                break;
            case 5:
                mat.color = Color.gray;
                vials[2].SetActive(false);
                break;

            default:
                mat.color = Color.white;
                break;

        }



    }
}
