using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    private void Start(){
        //Connect to the photon network using the stored server settings
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster(){
        //Once the connection has been established change scene to main menu
        SceneManager.LoadScene("Main Menu");
    }
}
