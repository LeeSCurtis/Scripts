using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class AreaExit : MonoBehaviour
{
    public string areaToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (PhotonView.Get(other).IsMine)
            {
                PhotonNetwork.LoadLevel(areaToLoad);
            }
        }
    }
}
