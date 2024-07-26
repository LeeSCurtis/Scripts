using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject player;
    public float minX, minZ, maxX, maxZ;
    private void Start(){
        Vector3 randomPosition = new Vector3(Random.Range(minX, maxX),Random.Range(minZ,maxZ));
        PhotonNetwork.Instantiate(player.name, randomPosition,Quaternion.Euler(0,0,0));
        //Instantiation must be an object from the Resources folder for PUN
    }
}
