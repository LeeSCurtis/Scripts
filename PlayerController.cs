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
    PhotonView playervView;

    void Start()    {
        rb = gameObject.GetComponent<Rigidbody>(); // Get the Rigidbody component
        playervView = GetComponent<PhotonView>();

    }

    void Update()    {
        if (playervView.IsMine)
        {
            RaycastHit hit;
            Vector3 castPosition = transform.position;
            castPosition.y += 1;

            // Perform a downward raycast that only detects the terrain layer
            if (Physics.Raycast(castPosition, -transform.up, out hit, Mathf.Infinity, terrainLayer))        {
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
            if (horizontalInput != 0 || verticalInput != 0)        {
                myAnim.SetFloat("lastMoveX", verticalInput);
                myAnim.SetFloat("lastMoveZ", horizontalInput);
            }
        }
    }
}
