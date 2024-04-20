using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGobbieAxe : MonoBehaviour
{
    public float damage;
    private Collider2D col;
    private SpriteRenderer spriteRenderer;
    private Vector2 startingDirection;
    void Start()
    {
        col = GetComponent<Collider2D>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

        // get current direction on where this is going and based on that flip the sprite
        Vector2 direction = (GameObject.FindWithTag("Player").transform.position - transform.position).normalized;
        startingDirection = direction;
        FlipSprite(direction);
    }

    void FlipSprite(Vector2 dir)
    {
        if (dir.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (dir.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    void Update()
    {
        if (startingDirection.x > 0)
        {
            transform.GetChild(0).Rotate(0, 0, -420 * Time.deltaTime);
        }
        else
        {
            transform.GetChild(0).Rotate(0, 0, 420 * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // take the other object isInvincible bool if false, deal damage
            if (!other.GetComponent<CharacterMovement>().isInvincible)
            {
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                playerHealth.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
