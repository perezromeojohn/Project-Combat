using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGobbie : MonoBehaviour
{
    private GameObject player;
    private Behavior behavior;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public EnemyStates currentState = EnemyStates.Moving;
    public GameObject projectile;

    void Start()
    {
        // Get the player with the Player tag
        player = GameObject.FindWithTag("Player");
        behavior = GetComponent<Behavior>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyStates.Moving:
                if (!behavior.isAttacked && behavior.health > 0)
                {
                    MoveToPlayer(player.transform);
                }
                break;
            case EnemyStates.Attacking:
                if (!behavior.isAttacked && behavior.health > 0)
                {
                    Attack();
                }
                break;
            case EnemyStates.Dead:
                if (behavior.health <= 0)
                {
                    currentState = EnemyStates.Dead;
                }
                break;
        }
    }

    void MoveToPlayer(Transform target)
    {
        // if enemy transform is close to player transform, change state to attacking
        Vector2 direction = (target.position - transform.position).normalized;
        FlipSprite(direction);

        if (Vector2.Distance(transform.position, target.position) < 1f)
        {
            SetState(EnemyStates.Attacking);
        } else {
            SetState(EnemyStates.Moving);
            if (!behavior.isAttacked && behavior.health > 0)
            {
                transform.Translate(direction * behavior.movementSpeed * Time.deltaTime);
            }
        }
    }

    void Attack()
    {
        if (animator.GetBool("isAttacking") == true)
        {
            return;
        }
        animator.SetBool("isAttacking", true);
        Debug.Log("Attacking");
        animator.Play("attack");
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

    // Method to change the enemy state
    void SetState(EnemyStates state)
    {
        currentState = state;
    }

    public void ResetAttack()
    {
        animator.SetBool("isAttacking", false);
        SetState(EnemyStates.Moving);
    }

    // animation events
    void SpawnProjectile()
    {
        GameObject projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);
        Vector2 direction = player.transform.position - transform.position;
        LeanTween.move(projectileInstance, (Vector2)transform.position + direction.normalized * 3, 3f)
                    .setEaseLinear()
                    .setOnComplete(() => Destroy(projectileInstance));
    }
}