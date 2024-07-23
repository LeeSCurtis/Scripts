using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Diagnostics.CodeAnalysis;
using TMPro;

public class MainMenu : MonoBehaviourPunCallbacks{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;
    //Hold a reference to the input fields 
    public void CreateRoom(){
        RoomOptions roomOptions = new RoomOptions();
        //Initialise room options
        roomOptions.MaxPlayers = 4;
        //Set the max amount of players in the room
        PhotonNetwork.CreateRoom(createInput.text,roomOptions);
        //Pass through the roomname, room options and create the room      
    }

    public void JoinRoom(){
        PhotonNetwork.JoinRoom(joinInput.text);
        //Join the room using the provided text input could check for empty?
    }

    public override void OnJoinedRoom(){
        PhotonNetwork.LoadLevel("Game");
        /*
            It's important to make sure to use 
            PhotonNetwork.LoadLevel(); 
            when moving players over the network
            instead of the unity scene manager
        */
    }
}
