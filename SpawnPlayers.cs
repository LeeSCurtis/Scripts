using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class SpawnPlayers : MonoBehaviour{
    public GameObject playerPrefab; // Reference to your player prefab
    public Transform[] spawnPoints; // Array of spawn points (set in the Unity hierarchy)

    private void Start()
    {
        // Choose a random spawn point
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Vector3 randomPosition = spawnPoints[randomIndex].position;

        // Instantiate the player at the chosen position
        PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
        // Note: Instantiation must use an object from the Resources folder for PUN
    }
}