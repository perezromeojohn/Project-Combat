using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetMovement : MonoBehaviour
{
    private GameObject target; // Target GameObject for the pet to follow
    public float moveSpeed = 2f; // Speed of movement
    public float stopSpacing = .5f; // Spacing value when stopped

    private float lerpSpeed = 1f; // Speed of interpolation
    private Vector3 targetPosition; // Target position for movement
    private bool isFollowing = true; // Flag to indicate whether the pet is following the target

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        
        if (target != null)
        {
            targetPosition = target.transform.position;
        }
        else
        {
            Debug.LogError("No target GameObject specified for the pet!");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    void Update()
    {
        if (isFollowing)
        {
            if (target != null)
            {
                targetPosition = target.transform.position;
            }

            Vector3 direction = targetPosition - transform.position;
            if (direction.magnitude > stopSpacing)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);
            }
        }

        if (targetPosition.x < transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    public void SetFollowState(bool follow)
    {
        isFollowing = follow;
    }
}
