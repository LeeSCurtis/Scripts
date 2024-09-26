using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviourPunCallbacks{
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
    public HealthManager healthManager; //Hold a reference to the health manager script
    private Camera myCamera; // Reference to the Camera component
    private bool isMoving; // A boolean to check if the player is moving
    private Collider myCollider; // Reference to the Collider component

    void Start()
    {
        if (photonView.IsMine)
        {
            myCamera = transform.GetChild(0).GetComponent<Camera>(); // Get the Camera component as a child of the player
            myCamera.gameObject.SetActive(true); // Set the camera to active
            myAnim = GetComponent<Animator>(); // Get the Animator component
            playerNumber = PhotonNetwork.CurrentRoom.PlayerCount; //Set the player number to the current player count     
            rb = gameObject.GetComponent<Rigidbody>(); // Get the Rigidbody component
            playerView = GetComponent<PhotonView>(); // Get the PhotonView component
            healthManager = GameObject.Find("Health").GetComponent<HealthManager>(); //get the healthmanager object
            sr = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
            myCollider = GetComponent<Collider>(); // Get the Collider component
        }
    }
    void Update(){
        if (photonView.IsMine)
        {
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
                myAnim.SetBool("isMoving", true);
                
                // Control looking left or right
                if (horizontalInput > 0)
                {
                    sr.flipX = false;
                }else if (horizontalInput < 0)
                {
                    sr.flipX = true;
                }
                myAnim.SetFloat("lastMoveX", verticalInput);
                myAnim.SetFloat("lastMoveZ", horizontalInput);
            } 
                else
            {
                myAnim.SetBool("isMoving", false);
            } 
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (playerView.IsMine)
        {
            if (other.CompareTag("Enemy")) // Check if the collider belongs to the player
            {
                playerView.RPC("PlayerTakeDamage", RpcTarget.All, 10, playerNumber);
                other.GetComponent<EnemyScript>().DestroyOnNetwork();
            }
        }
    }
    // On collision with another collider of tag "player" ignore the collision
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Physics.IgnoreCollision(collision.collider, myCollider);
        }
    }
    void OnFire()
    {
        if (playerView.IsMine)
        {
            // Call the TakeDamage RPC
            playerView.RPC("PlayMeleeAnimation", RpcTarget.All, 10, playerNumber);
        }
    }
    [PunRPC]
    public void PlayMeleeAnimation(int damage, int playerNumber)
    {
        // Activate the trigger for the melee attack animation
        myAnim.SetTrigger("meleeAttack");
    }
    // Take damage RPC to let the health manager know that the player has taken damage
    [PunRPC]
    public void PlayerTakeDamage(int damage, int playerNumber) // How much damage and who should take the damage
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