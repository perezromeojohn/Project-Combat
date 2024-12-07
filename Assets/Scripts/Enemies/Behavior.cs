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
    private AudioSource hitSound;

    private TimeManager timeManager;
    private float currentLevel = 1;
    private float healthMultiplier = 1.2f;


    private void Awake()
    {
        damageNumbers = DamageNumberManager.Instance;
        timeManager = GlobalTimeManager.Instance;
    }

    // events
    private void Start()
    {
        EnemyInit();
        onSpawn.Invoke();
    }

    IEnumerator EnemyHit()
    {
        if (isAttacked && !isHit)
        {
            isHit = true;
            onHit.Invoke();
            // play hit sound with random pitch
            hitSound.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            hitSound.Play();
            TakeDamage(damageTaken);
            damageNumbers.DamageValues(transform, damageTaken);
            // Instantiate(hitParticles, transform.position, Quaternion.identity);
            GameObject particles = Instantiate(hitParticles, transform.position, Quaternion.identity);
            ParticleSystem.VelocityOverLifetimeModule velocity = particles.GetComponent<ParticleSystem>().velocityOverLifetime;
            Vector2 direction = (transform.position - player.position).normalized;
            velocity.x = direction.x * 1f;
            velocity.y = direction.y * 1f;
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
        hitSound = GetComponent<AudioSource>();

        gameObject.name = enemy.enemyName;
        // set mainEnemyHealthBar position to the healthbarUIOffset
        mainEnemyHealthBar.transform.position = transform.position + healthBarUIOffset;
    }

    void SetStats()
    {
        var elapsedTime = timeManager.GetTimeElapsed();
        currentLevel = Mathf.FloorToInt(elapsedTime / 60) + 1;
        maxHealth = Mathf.Floor(enemy.maxHealth * Mathf.Pow(healthMultiplier, currentLevel - 1));
        health = maxHealth;
        damage = enemy.damage;
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
            MobDeath();
        }
    }

    public void MobDeath()
    {
        KillCount.AddKill();
        Animator enemyAnimator = GetComponent<Animator>();
        enemyAnimator.SetBool("isHealthZero", true);
        enemyHealthBarUI.enabled = false;
        onDeathStart.Invoke();
        enemyHealthBar.GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
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
