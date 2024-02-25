using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    // get character playerStats
    public PlayerStats playerStats;
    public Collider2D playerCollider;
    public HealthBar healthBar;
    private float maxHealth;
    private float health;

    // events
    public UnityEvent OnPlayerHit;
    public UnityEvent OnPlayerDeath;

    // on start, print player health
    void Start()
    {
        maxHealth = playerStats.maxHealth;
        health = maxHealth;
        healthBar.DrawHearts(health, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        Debug.Log(damage);
        health -= damage;
        Debug.Log(health);
        OnPlayerHit.Invoke();
        healthBar.DrawHearts(health, maxHealth);
    }

    void Die ()
    {
        OnPlayerDeath.Invoke();
    }
}
