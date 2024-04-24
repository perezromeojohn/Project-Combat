using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skellie : MonoBehaviour
{
    private GameObject player;
    private Behavior behavior;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        // get the player with the Player tag
        player = GameObject.FindWithTag("Player");
        behavior = GetComponent<Behavior>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPlayer(player.transform);
    }

    void MoveToPlayer(Transform target)
    {
        Vector2 direction = (target.position - transform.position).normalized;
        FlipSprite(direction);

        // Move towards the player
        // transform.Translate(direction * behavior.movementSpeed * Time.deltaTime);
        if (!behavior.isAttacked && behavior.health > 0)
        {
            // transform.Translate(direction * behavior.movementSpeed * Time.deltaTime);
            // use rigidbody2d to move the enemy
            rb.MovePosition(rb.position + direction * behavior.movementSpeed * Time.fixedDeltaTime);
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
