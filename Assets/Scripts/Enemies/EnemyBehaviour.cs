using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Enemy Stats")]
    public List<Enemy> enemyList;
    private Enemy selectedEnemy;
    private Transform player;
    [SerializeField] private float health;
    private float maxHealth;
    
    [Header("Attack Settings")]
    public bool isAttacked = false;
    private bool isHit = false;
    public float damageTaken = 0f;

    [Header("Enemy UI")]
    public SpriteRenderer enemyHealthBarUI;
    public GameObject enemyHealthBar;
    public SpriteRenderer spriteRenderer;


    void Start()
    {
        int randomIndex = UnityEngine.Random.Range(0, enemyList.Count);
        selectedEnemy = enemyList[randomIndex];

        // set stats
        health = selectedEnemy.health;
        maxHealth = selectedEnemy.maxHealth;

        SetHealthUI();

        // Find the player GameObject by tag ("Player")
        player = GameObject.FindWithTag("Player").transform; // we are referencing the player's transform, not the last position of the player
        // get animator
        string animatorPath = "Assets/Animations/EnemyOverrides/" + selectedEnemy.enemyName + ".overrideController";
        Animator enemyAnimator = GetComponent<Animator>();
        
        if (File.Exists(animatorPath))
        {
            // load animator
            AnimatorOverrideController animatorOverrideController = (AnimatorOverrideController)AssetDatabase.LoadAssetAtPath(animatorPath, typeof(AnimatorOverrideController));
            enemyAnimator.runtimeAnimatorController = animatorOverrideController;
        }
        else
        {
            Debug.LogWarning("Animator not found for enemy: " + selectedEnemy.enemyName);
        }

        gameObject.name = selectedEnemy.enemyName;
    }

    void Update()
    {
        StartCoroutine(EnemyHit());


        if (player != null)
        {
            // Calculate the direction to the player
            Vector3 direction = (player.position - transform.position).normalized;
            
            // Calculate the distance to the player
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > selectedEnemy.minDistanceToPlayer && health > 0f)
            {
                // Move the enemy towards the player's current position
                transform.Translate(direction * selectedEnemy.movementSpeed * Time.deltaTime);
            } else {
                // Debug.Log("Attack the player");
            }

            // Check if the enemy is moving left and flip the sprite accordingly
            if (direction.x < 0 && health > 0f)
            {
                spriteRenderer.flipX = true;
            }
            else if (direction.x > 0 && health > 0f)
            {
                spriteRenderer.flipX = false;
            }
        }
    }

    void SetHealthUI() {
        if (health == maxHealth)
        {
            enemyHealthBarUI.enabled = false;
            enemyHealthBar.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            enemyHealthBarUI.enabled = true;
            enemyHealthBar.GetComponent<SpriteRenderer>().enabled = true;
        }
        float scaleFactor = Mathf.Clamp01(health / maxHealth);
        Vector3 newScale = enemyHealthBar.transform.localScale;
        newScale.x = scaleFactor;
        enemyHealthBar.transform.localScale = newScale;
    }

    public void TakeDamage(float damage)
    {
        health -= damageTaken;
        health = Mathf.Clamp(health, 0f, maxHealth);
        SetHealthUI();
        if (health <= 0f)
        {
            // get the animtor's bool value of isHealthZero and set it to true
            Animator enemyAnimator = GetComponent<Animator>();
            enemyAnimator.SetBool("isHealthZero", true);
            enemyHealthBarUI.enabled = false;
            enemyHealthBar.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    IEnumerator EnemyHit()
    {
        if (isAttacked && !isHit)
        {
            isHit = true; 
            TakeDamage(damageTaken);
            yield return new WaitForSeconds(0.5f);
            isAttacked = false;
            damageTaken = 0f;
            isHit = false;
        }
    }
}
