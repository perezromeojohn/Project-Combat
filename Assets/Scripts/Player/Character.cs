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

    [Header("Player Stats")]
    public PlayerStats playerStats;

    [Header("Player Movement")]
    private Rigidbody2D rb;
    [SerializeField] private Vector3 moveDirection;

    [Header("Player Blink")]
    private bool isDashButtonDown;
    private float blinkTimer;

    [Header("Player Dodge Roll")]
    private Vector3 rollDirection;
    private float rollSpeed;
    [SerializeField] private CapsuleCollider2D swordCollider;
    public bool isInvincible = false;
    private State state;
    private float rollTimer;
    public GameObject dashSmoke;

    [Header("Player Animations")]

    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator hairAnimator;
    
    [Header("Player Attacking")]

    [SerializeField] private GameObject hand;
    [SerializeField] private Animator swordAnimator;
    private float attackTimer = 0f;


    public bool isAttacked = false;
    private bool isHit = false;
    public float damageTaken = 0f;
    public GameObject debris;

    void Start()
    {
        blinkTimer = 1f; // Initialize the Blink cooldown timer
        rollTimer = 1f; // Initialize the Dodge Roll cooldown timer
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        state = State.Normal;
    }

    void Update()
    {
        Attack();
        GetInput();
        Blink();

        swordAnimator.enabled = true;
        playerAnimator.enabled = true;
        hairAnimator.enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;

        // takeDamage
        // StartCoroutine(PlayerHit());
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case State.Normal:
                rb.velocity = moveDirection * playerStats.movementSpeed;
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
                    rb.MovePosition(transform.position + moveDirection * playerStats.blinkAmount);
                    isDashButtonDown = false;
                    blinkTimer = playerStats.blinkCooldown; // Set the Blink cooldown timer
                }

                // set hand gameobject to active
                hand.SetActive(true);

                break;
            case State.Rolling:
                rb.velocity = rollDirection * rollSpeed;
                playerAnimator.SetBool("isRolling", true);
                hairAnimator.SetBool("isRolling", true);
                hand.SetActive(false);
                swordCollider.enabled = false;
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
        attackTimer += Time.deltaTime;

        if (attackTimer >= 1f / playerStats.attackSpeed)
        {
            if (swordAnimator.GetBool("isAttacking") == false)
            {
                swordAnimator.SetBool("isAttacking", true);
            }

            attackTimer = 0f;
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

                isInvincible = false;
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
            if (moveDirection == Vector3.zero) return; // If moveDirection is zero, return
            rollDirection = moveDirection;
            rollSpeed = playerStats.rollSpeed;
            state = State.Rolling;
            rollTimer = playerStats.rollCooldown; // Set the Dodge Roll cooldown timer
            isInvincible = true;
            GameObject dashSmokeInstance = Instantiate(dashSmoke, transform.position, Quaternion.identity);
            dashSmokeInstance.transform.SetParent(debris.transform);
            // get the spriterendered and flip it depending on the direction the player is going
            if (rollDirection.x > 0)
            {
                dashSmokeInstance.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (rollDirection.x < 0)
            {
                dashSmokeInstance.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

    IEnumerator PlayerHit()
    {
        if (isAttacked && !isHit)
        {
            isHit = true;
            yield return new WaitForSeconds(.5f);
            isAttacked = false;
            damageTaken = 0f;
            isHit = false;
        }
    }
}
