using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinGobbie : MonoBehaviour
{
    private GameObject player;
    private Behavior behavior;
    private SpriteRenderer spriteRenderer;
    public float attackRange = 1f; // Adjust this value as needed
    private enum StateEnum { Walking, Attacking }
    private StateEnum currentState;
    public float circleRadius = 4f; // Adjust this value to change the circle radius
    private bool canFire = true;
    public GameObject projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        // get the player with the Player tag
        player = GameObject.FindWithTag("Player");
        behavior = GetComponent<Behavior>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentState = StateEnum.Walking;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case StateEnum.Walking:
                if (!behavior.isAttacked && behavior.health > 0)
                {
                    WalkToPlayer();
                }
                break;
            case StateEnum.Attacking:
                if (!behavior.isAttacked && behavior.health > 0)
                {
                    AttackPlayer(); // prime this shit
                    StartCoroutine(PrintFire());
                }
                break;
        }
        FlipSprite(player.transform.position - transform.position);
    }

    void WalkToPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        FlipSprite(direction);

        if (!behavior.isAttacked && behavior.health > 0)
        {
            transform.Translate(direction * behavior.movementSpeed * Time.deltaTime);
        }

        float distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);
        if (distanceToPlayer <= attackRange)
        {
            currentState = StateEnum.Attacking;
        }
    }

    IEnumerator PrintFire()
    {
        while (canFire)
        {
            canFire = false;
            Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(3f);
            canFire = true;
        }
    }

    void AttackPlayer()
    {
        Vector2 direction = player.transform.position - transform.position;
        float distanceToPlayer = direction.magnitude;

        if (distanceToPlayer > attackRange)
        {
            currentState = StateEnum.Walking;
        }
        else
        {
            // Calculate the orbit direction
            Vector2 orbitDirection = new Vector2(direction.y, -direction.x).normalized;

            // Calculate the desired orbit position
            Vector2 playerPosition2D = new Vector2(player.transform.position.x, player.transform.position.y);
            Vector2 desiredOrbitPosition = playerPosition2D - (orbitDirection * circleRadius);

            // Adjust the enemy's position to maintain the circle radius
            Vector2 moveDirection = (desiredOrbitPosition - (Vector2)transform.position).normalized;
            transform.position = (Vector2)transform.position + (moveDirection * behavior.movementSpeed * Time.deltaTime);
        }
    }

    void FlipSprite(Vector2 dir)
    {
        if (dir.x > 0 && behavior.health > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (dir.x < 0 && behavior.health > 0)
        {
            spriteRenderer.flipX = true;
        }
    }
}

