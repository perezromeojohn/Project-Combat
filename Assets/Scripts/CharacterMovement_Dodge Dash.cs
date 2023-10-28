using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    // State Machine
    private enum State {
        Normal,
        Rolling
    }

    [Header("Player Movement")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private float moveSpeed = 2f;

    // i hate math physics
    [Header("Player Blink")]
    [SerializeField] private float blinkAmount = 3f;
    private bool isDashButtonDown;

    [Header("Player Dodge Roll")]
    
    [SerializeField] private Vector3 rollDirection;
    [SerializeField] private float rollSpeed = 7f;
    private State state;

    

    void Start()
    {
        
    }

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        state = State.Normal;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Blink();
    }

    void FixedUpdate() {
        switch (state) {
        case State.Normal:
            rb.velocity = moveDirection * moveSpeed;

            if (isDashButtonDown) {
                rb.MovePosition(transform.position + moveDirection * blinkAmount);
                isDashButtonDown = false;
            }
                
            break;
        case State.Rolling:
            rb.velocity = rollDirection * rollSpeed;
            break;
        }
    }

    void GetInput() {
        switch (state) {
        case State.Normal:
            float moveX = 0f;
            float moveY = 0f;

            if (Input.GetKey(KeyCode.W)) {
                moveY = 1f;
            }
            if (Input.GetKey(KeyCode.S)) {
                moveY = -1f;
            }
            if (Input.GetKey(KeyCode.A)) {
                moveX = -1f;
            }
            if (Input.GetKey(KeyCode.D)) {
                moveX = 1f;
            }

            moveDirection = new Vector3(moveX, moveY).normalized;
            DodgeRoll();
            break;
        case State.Rolling:
            float rollSpeedDropMultiplier = 5f;
            rollSpeed -= rollSpeed * rollSpeedDropMultiplier * Time.deltaTime;

            float rollSpeedMinimum = 1f;
            if (rollSpeed < rollSpeedMinimum) {
                state = State.Normal;
            }
            break;
        }
    }

    void Blink() {
        if (Input.GetKey(KeyCode.F)) {
            isDashButtonDown = true;
        }
    }

    void DodgeRoll() {
        if (Input.GetKey(KeyCode.Space)) {
            rollDirection = moveDirection;
            rollSpeed = 5;
            state = State.Rolling;
        }
    }
}
