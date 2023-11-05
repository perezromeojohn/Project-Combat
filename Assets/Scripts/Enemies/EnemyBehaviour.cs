using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    // player stuff
    [Header("Player Stuff")]
    public PlayerStats playerStats;
    private GameObject playerGameObject;

    [Header("Enemy Stats")]
    public List<Enemy> enemyList;
    private Enemy selectedEnemy;
    private Transform player;
    private Color color;
    private Vector3 healthBarUIOffset;
    private SpriteRenderer enemySprite;
    [SerializeField] private float health;
    private float maxHealth;
    
    [Header("Attack Settings")]
    public bool isAttacked = false;
    private bool isHit = false;
    public float damageTaken = 0f;
    private float damage;

    [Header("Enemy UI")]
    public SpriteRenderer enemyHealthBarUI;
    public GameObject enemyHealthBar;
    public GameObject mainEnemyHealthBar;
    public SpriteRenderer spriteRenderer;


    public FlashHit flashHit;
    public Knockback knockback;
    private bool hasDied = false;


    void Start()
    {
        int randomIndex = UnityEngine.Random.Range(0, enemyList.Count);
        selectedEnemy = enemyList[randomIndex];

        // set stats
        SetStats();

        SetHealthUI();

        playerGameObject = GameObject.FindWithTag("Player");
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

        enemySprite = GetComponent<SpriteRenderer>();
        enemySprite.color = color;

        gameObject.name = selectedEnemy.enemyName;
        // set mainEnemyHealthBar position to the healthbarUIOffset
        mainEnemyHealthBar.transform.position = transform.position + healthBarUIOffset;
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

            if (distanceToPlayer > selectedEnemy.minDistanceToPlayer && health > 0f && !isHit)
            {
                // Move the enemy towards the player's current position
                transform.Translate(direction * selectedEnemy.movementSpeed * Time.deltaTime);
            } else if (distanceToPlayer <= selectedEnemy.minDistanceToPlayer) {
                // Debug.Log("Attack the player");
                if (playerGameObject.GetComponent<CharacterMovement>().isInvincible == false && !hasDied)
                {
                    playerGameObject.GetComponent<CharacterMovement>().damageTaken = damage; // This script is somehowcalled CharacterMovement even though its only named Character
                    playerGameObject.GetComponent<CharacterMovement>().isAttacked = true;
                }
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

    void SetStats() {
        health = selectedEnemy.health;
        maxHealth = selectedEnemy.maxHealth;
        damage = selectedEnemy.damage;
        color = selectedEnemy.spriteColor;
        healthBarUIOffset = selectedEnemy.healthBarUIOffset;
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
        LeanTween.scaleX(enemyHealthBar, scaleFactor, 0.3f).setEase(LeanTweenType.easeOutQuart);
    }

    public void TakeDamage(float damage)
    {
        health -= damageTaken;
        health = Mathf.Clamp(health, 0f, maxHealth);
        SetHealthUI();
        flashHit.Flash();
        if (health <= 0f)
        {
            // get the animtor's bool value of isHealthZero and set it to true
            Animator enemyAnimator = GetComponent<Animator>();
            enemyAnimator.SetBool("isHealthZero", true);
            enemyHealthBarUI.enabled = false;
            hasDied = true;
            enemyHealthBar.GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().simulated = false;
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
            knockback.AddForce(player);
            yield return new WaitForSeconds(0.3f);
            isAttacked = false;
            damageTaken = 0f;
            isHit = false;
        }
    }
}
