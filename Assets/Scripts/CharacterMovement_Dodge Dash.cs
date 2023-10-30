using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private enum State
    {
        Normal,
        Rolling
    }

    [Header("Player Movement")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private float moveSpeed = 2f;

    [Header("Player Blink")]
    [SerializeField] private float blinkAmount = 3f;
    private bool isDashButtonDown;
    private float blinkCooldown = 2f; // Cooldown for Blink
    private float blinkTimer;

    [Header("Player Dodge Roll")]
    [SerializeField] private Vector3 rollDirection;
    [SerializeField] private float rollSpeed = 7f;
    private State state;
    private float rollCooldown = 1f; // Cooldown for Dodge Roll
    private float rollTimer;

    [Header("Player Animations")]

    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator hairAnimator;
    
    [Header("Player Attacking")]

    [SerializeField] private GameObject hand;
    [SerializeField] private Animator swordAnimator;


    void Start()
    {
        blinkTimer = 3f; // Initialize the Blink cooldown timer
        rollTimer = 1f; // Initialize the Dodge Roll cooldown timer
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        state = State.Normal;
    }

    void Update()
    {
        GetInput();
        Blink();
        Attack();
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case State.Normal:
                rb.velocity = moveDirection * moveSpeed;
                // set animation of player and hair to run, if not moving, set it both to idle
                if (moveDirection.x != 0 || moveDirection.y != 0)
                {
                    playerAnimator.SetBool("isRunning", true);
                    hairAnimator.SetBool("isRunning", true);
                }
                else
                {
                    playerAnimator.SetBool("isRunning", false);
                    hairAnimator.SetBool("isRunning", false);
                }

                playerAnimator.SetBool("isRolling", false);
                hairAnimator.SetBool("isRolling", false);

                if (isDashButtonDown && blinkTimer <= 0)
                {
                    rb.MovePosition(transform.position + moveDirection * blinkAmount);
                    isDashButtonDown = false;
                    blinkTimer = blinkCooldown; // Set the Blink cooldown timer
                }

                // set hand gameobject to active
                hand.SetActive(true);

                break;
            case State.Rolling:
                rb.velocity = rollDirection * rollSpeed;
                playerAnimator.SetBool("isRolling", true);
                hairAnimator.SetBool("isRolling", true);
                hand.SetActive(false);
                break;
        }

        // Update the cooldown timers
        if (blinkTimer > 0)
        {
            blinkTimer -= Time.deltaTime;
        }

        if (rollTimer > 0)
        {
            rollTimer -= Time.deltaTime;
        }
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (swordAnimator.GetBool("isAttacking") == false)
            {
                swordAnimator.SetBool("isAttacking", true);
            }
        }
    }

    void GetInput()
    {
        switch (state)
        {
            case State.Normal:
                float moveX = 0f;
                float moveY = 0f;

                if (Input.GetKey(KeyCode.W))
                {
                    moveY = 1f;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    moveY = -1f;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    moveX = -1f;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    moveX = 1f;
                }

                moveDirection = new Vector3(moveX, moveY).normalized;
                DodgeRoll();
                break;
            case State.Rolling:
                float rollSpeedDropMultiplier = 5f;
                rollSpeed -= rollSpeed * rollSpeedDropMultiplier * Time.deltaTime;
                // set animation of player and hair to roll, if not moving, set it both to idle

                float rollSpeedMinimum = 1f;
                if (rollSpeed < rollSpeedMinimum)
                {
                    state = State.Normal;
                }
                break;
        }
    }

    void Blink()
    {
        if (Input.GetKey(KeyCode.F) && blinkTimer <= 0)
        {
            isDashButtonDown = true;
        }
    }

    void DodgeRoll()
    {
        if (Input.GetKey(KeyCode.Space) && rollTimer <= 0)
        {
            rollDirection = moveDirection;
            rollSpeed = 5;
            state = State.Rolling;
            rollTimer = rollCooldown; // Set the Dodge Roll cooldown timer
        }
    }
}
