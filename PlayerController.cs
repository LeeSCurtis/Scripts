using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class PlayerController : MonoBehaviour{
    public float speed; // A variable for modifying speed
    public float groundDistance; // Track the player's distance from the ground
    public LayerMask terrainLayer; // The layer mask for detecting terrain
    public Rigidbody rb; // Reference to the Rigidbody component
    public SpriteRenderer sr; // Reference to the SpriteRenderer component
    private float horizontalInput; //The horizontal input value
    private float verticalInput; //The vertical input value
    public Animator myAnim; //Reference to the animator
    PhotonView playerView; // Reference to the PhotonView component

    public int playerNumber; // The player number

    //Hold a reference to the health manager script
    public HealthManager healthManager;

    void Start()
    {
        //If the masterclient is the player set player number to 1
        if (PhotonNetwork.IsMasterClient)
        {
            playerNumber = 1;
            Debug.Log("Player number: " + playerNumber);
        }
        else //if the client is not the master then check how many players are in the room and set the player number accordingly
        {
            playerNumber = PhotonNetwork.CurrentRoom.PlayerCount;
            Debug.Log("Player number: " + playerNumber);
        }
        
        rb = gameObject.GetComponent<Rigidbody>(); // Get the Rigidbody component
        playerView = GetComponent<PhotonView>(); // Get the PhotonView component
        //get the healthmanager object
        healthManager = GameObject.Find("Health").GetComponent<HealthManager>();
        //If the masterclient is the player set player number to 1
        if (PhotonNetwork.IsMasterClient)
        {
            playerNumber = 1;
        }
        else //if the client is not the master then check how many players are in the room and set the player number accordingly
        {
            playerNumber = PhotonNetwork.CurrentRoom.PlayerCount;
            Debug.Log("Player number: " + playerNumber);
        }
    }

    void Update(){
        if (playerView.IsMine)
        {
            RaycastHit hit;
            Vector3 castPosition = transform.position;
            castPosition.y += 1;

            // Perform a downward raycast that only detects the terrain layer
            if (Physics.Raycast(castPosition, -transform.up, out hit, Mathf.Infinity, terrainLayer)){
                // If it hits the terrain, move the character above
                if (hit.collider != null)            {
                    Vector3 movePosition = transform.position;
                    movePosition.y = hit.point.y + groundDistance;
                    transform.position = movePosition;
                }
            }

            // Get input values
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            // Calculate movement direction
            Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);

            // Apply velocity to the Rigidbody
            rb.velocity = moveDirection * speed;

            // Set animation parameters
            myAnim.SetFloat("moveX", rb.velocity.x);
            myAnim.SetFloat("moveZ", rb.velocity.z);

            // Update last move parameters only when there's input
            if (horizontalInput != 0 || verticalInput != 0)        
            {
                myAnim.SetFloat("lastMoveX", verticalInput);
                myAnim.SetFloat("lastMoveZ", horizontalInput);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (playerView.IsMine)
        {
            if (other.CompareTag("Enemy")) // Check if the collider belongs to the player
            {
                // Call the TakeDamage RPC
                playerView.RPC("PlayerTakeDamage", RpcTarget.All, 10, playerNumber);
                // Destroy the player
                //PhotonNetwork.Destroy(gameObject);
                // Destroy other.gameObject
                other.GetComponent<EnemyScript>().DestroyOnNetwork();
            }
        }
    }

    // Take damage RPC to let the health manager know that the player has taken damage
    [PunRPC]
    public void PlayerTakeDamage(int damage, int playerNumber) // Who and how much damage to take
    {
        switch (playerNumber) // Check the player number and reduce the health accordingly
        {
            case 1:
                healthManager.player1Health -= damage;
                break;
            case 2:
                healthManager.player2Health -= damage;
                break;
            case 3:
                healthManager.player3Health -= damage;
                break;
            case 4:
                healthManager.player4Health -= damage;
                break;
        }
        // Call the update health text method in the health manager
        healthManager.UpdateHealthTextRPC();
    }   
}
