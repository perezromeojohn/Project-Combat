using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Enemy Stats")]
    public Enemy enemy;
    private Transform player;


    void Start()
    {
        // Find the player GameObject by tag ("Player")
        player = GameObject.FindWithTag("Player").transform; // we are referencing the player's transform, not the last position of the player
        // get animator
        Animator animator = GetComponent<Animator>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        Debug.Log(enemy.damage);
    }

    void Update()
    {
        if (player != null)
        {
            // Calculate the direction to the player
            Vector3 direction = (player.position - transform.position).normalized;
            
            // Calculate the distance to the player
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > enemy.minDistanceToPlayer)
            {
                // Move the enemy towards the player's current position
                transform.Translate(direction * enemy.movementSpeed * Time.deltaTime);
            } else {
                // Debug.Log("Attack the player");
            }

            // Check if the enemy is moving left and flip the sprite accordingly
            if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (direction.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
