using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using MoreMountains.Feedbacks;

public class Behavior : MonoBehaviour
{
    public UnityEvent onHit;
    public UnityEvent onDeathEnd;
    public UnityEvent onDeathStart;
    public UnityEvent onSpawn;

    [Header("Enemy Stats")]
    public Enemy enemy; // scriptable object
    private Transform player;
    private Color color;
    private Vector3 healthBarUIOffset;
    private SpriteRenderer enemySprite;
    public Animator enemyAnimator;
    public float health;
    public float maxHealth;
    public float damage;
    public float movementSpeed;
    public float damageTaken = 0f;
    private bool isHit = false;
    public bool isAttacked = false;
    
    [Header("Enemy UI")]
    public SpriteRenderer enemyHealthBarUI;
    public GameObject enemyHealthBar;
    public GameObject mainEnemyHealthBar;
    public SpriteRenderer spriteRenderer;

    [Header("Enemy Hit Feedback")]
    public GameObject hitParticles;
    private DamageNumbers damageNumbers;

    // events
    private void Start()
    {
        EnemyInit();
        onSpawn.Invoke();
        damageNumbers = DamageNumberManager.Instance;
    }

    IEnumerator EnemyHit()
    {
        if (isAttacked && !isHit)
        {
            isHit = true;
            onHit.Invoke();
            TakeDamage(damageTaken);
            damageNumbers.DamageValues(transform, damageTaken);
            Instantiate(hitParticles, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            isAttacked = false;
            damageTaken = 0f;
            isHit = false;
        }
    }

    private void Die()
    {
        onDeathEnd.Invoke();
        Destroy(gameObject);
    }

    void EnemyInit()
    {
        SetStats();
        SetHealthUI();

        player = GameObject.FindWithTag("Player").transform; // we are referencing the player's transform, not the last position of the player
        
        enemySprite = GetComponent<SpriteRenderer>();
        enemySprite.color = color;

        gameObject.name = enemy.enemyName;
        // set mainEnemyHealthBar position to the healthbarUIOffset
        mainEnemyHealthBar.transform.position = transform.position + healthBarUIOffset;
    }

    void SetStats()
    {
        health = enemy.health;
        maxHealth = enemy.maxHealth;
        damage = enemy.damage;
        color = enemy.spriteColor;
        movementSpeed = enemy.movementSpeed;
        healthBarUIOffset = enemy.healthBarUIOffset;
    }
    
    void SetHealthUI()
    {
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
            Animator enemyAnimator = GetComponent<Animator>();
            enemyAnimator.SetBool("isHealthZero", true);
            enemyHealthBarUI.enabled = false;
            onDeathStart.Invoke();
            enemyHealthBar.GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().simulated = false;
        }
    }

    void Update()
    {
        StartCoroutine(EnemyHit());
    }

    // on collider with a Player tag debug log
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // take the other object isInvincible bool if false, deal damage
            if (!other.GetComponent<CharacterMovement>().isInvincible)
            {
                // get other PlayerHealth component
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                // take damage
                playerHealth.TakeDamage(damage);
                // Debug.Log(damage);s
            }
        }
    }
}
