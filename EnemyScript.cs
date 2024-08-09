using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyScript : MonoBehaviour
{
    PlayerController[] players;
    PlayerController nearestPlayer;
    public float speed;
    public float damageAmount = 100f;

    private void Start()
    {
        players = FindObjectsOfType<PlayerController>();
    }

    private void Update()
    {
        float minDistance = float.MaxValue; // Initialize with a large value
        
        foreach (var player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPlayer = player;
            }
        }

        // 'nearestPlayer' contains the closest player
        // The enemy will use this information to move towards the player
        if (nearestPlayer != null)
        {
            Vector3 directionToPlayer = nearestPlayer.transform.position - transform.position;
            directionToPlayer.Normalize(); // Normalize the direction vector

            // Move towards the nearest player at "speed"
            transform.position += directionToPlayer * speed * Time.deltaTime;
        }
    }
    
    public void DestroyOnNetwork()
    {
        // Call this method when you want to destroy the object
        // It will trigger an RPC to notify all clients to destroy it
        GetComponent<PhotonView>().RPC("NetworkDestroy", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void NetworkDestroy()
    {
        // Actual destruction logic here
        PhotonNetwork.Destroy(gameObject);
    }
}