using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{

    [Header("Player Properties")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Player Movement")]
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private float movementSpeed = 1f;

    [Header("Player Dashing")]
    [SerializeField] private float activeMoveSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashLength = 1f, dashCooldown = 1f; 
    private float dashCounter;
    private float dashCooldownCounter;
    

    void Start()
    {
        activeMoveSpeed = movementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Dash();
    }

    private void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // move the player based on the input
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * movementSpeed * Time.deltaTime);
        // flip the sprite based on the input
        if (horizontalInput < 0) {
            GetComponent<SpriteRenderer>().flipX = true;
        } else if (horizontalInput > 0) {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    private void Dash()
    {
        // create player movement for 2d
        // get the horizontal and vertical input from the player
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();
        if (Input.GetKeyDown(KeyCode.Space)) {

            rb.velocity = Vector2.zero; // Set velocity to zero before applying dash velocity
            rb.velocity = moveInput * activeMoveSpeed;
            if (dashCooldownCounter <= 0 && dashCounter <= 0) {
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;
            }
        }

        if (dashCounter > 0) {
            dashCounter -= Time.deltaTime;
            if (dashCounter <= 0) {
                activeMoveSpeed = movementSpeed;
                dashCooldownCounter = dashCooldown;
            }
        }

        if (dashCooldownCounter > 0) {
            dashCooldownCounter -= Time.deltaTime;
        }
    }
}
