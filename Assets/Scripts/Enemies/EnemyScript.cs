using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Enemy Stats")]
    public List<Enemy> enemyList;
    private Enemy selectedEnemy;
    private Transform player;


    void Start()
    {
        // Find the player GameObject by tag ("Player")
        player = GameObject.FindWithTag("Player").transform; // we are referencing the player's transform, not the last position of the player
        int randomIndex = Random.Range(0, enemyList.Count);
        selectedEnemy = enemyList[randomIndex];
        // get animator
        string animatorPath = "Assets/Animations/EnemyOverrides/" + selectedEnemy.enemyName + ".overrideController";
        Animator enemyAnimator = GetComponent<Animator>();
        
        if (File.Exists(animatorPath))
        {
            // load animator
            AnimatorOverrideController animatorOverrideController = (AnimatorOverrideController)AssetDatabase.LoadAssetAtPath(animatorPath, typeof(AnimatorOverrideController));
            enemyAnimator.runtimeAnimatorController = animatorOverrideController;
            Debug.Log("Found: " + animatorPath);
        }
        else
        {
            Debug.LogWarning("Animator not found for enemy: " + selectedEnemy.enemyName);
        }

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        Debug.Log(selectedEnemy.damage);
        // set gameObject name to enemy name
        gameObject.name = selectedEnemy.enemyName;
    }

    void Update()
    {
        if (player != null)
        {
            // Calculate the direction to the player
            Vector3 direction = (player.position - transform.position).normalized;
            
            // Calculate the distance to the player
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > selectedEnemy.minDistanceToPlayer)
            {
                // Move the enemy towards the player's current position
                transform.Translate(direction * selectedEnemy.movementSpeed * Time.deltaTime);
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
