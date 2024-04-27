using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herd : MonoBehaviour
{
    private Vector3 targetDirection;
    public float searchRadius = 1f;
    public float defaultRandomRange = 0.5f;
    public float walkSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        // Find nearby enemies
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, searchRadius);

        if (hitColliders.Length > 0)
        {
            // Find the nearest enemy
            Collider2D nearestEnemyCollider = null;
            float nearestDistance = float.MaxValue;

            foreach (Collider2D collider in hitColliders)
            {
                if (collider.gameObject.CompareTag("Enemy"))
                {
                    float distance = Vector2.Distance(transform.position, collider.transform.position);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestEnemyCollider = collider;
                    }
                }
            }

            if (nearestEnemyCollider != null)
            {
                // Calculate the direction towards the nearest enemy
                targetDirection = (nearestEnemyCollider.transform.position - transform.position).normalized;
            }
        }

        // If no enemy found or the logic couldn't set the target direction, randomize a direction
        if (targetDirection == Vector3.zero)
        {
            targetDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
        }

        // Start coroutine to destroy after 3 seconds
        StartCoroutine(DestroyAfterDelay(3.0f));

        // for loop get all active children, get sprite renderer, and flip the sprite's x axis
        for (int i = 0; i < transform.childCount; i++)
        {
            SpriteRenderer spriteRenderer = transform.GetChild(i).GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = targetDirection.x > 0;
        }
    }

    void Update()
    {
        // Move the fireball in the target direction
        transform.Translate(targetDirection * Time.deltaTime * walkSpeed, Space.World);
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
