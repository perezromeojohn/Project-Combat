using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Events;
using Unity.Mathematics;

public class PlayerHealth : MonoBehaviour
{
    // get character playerStats
    public PlayerStats playerStats;
    public Collider2D playerCollider;
    public HealthBar healthBar;
    private float maxHealth;
    private float health;

    private bool isAttacked = false;

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
        if (!isAttacked)
        {
            Debug.Log("Taking damage: " + damage);
            health -= damage;
            Debug.Log("Current health: " + health);
            OnPlayerHit.Invoke();
            healthBar.DrawHearts(health, maxHealth);

            isAttacked = true;
            StartCoroutine(ResetIsAttacked());
        }
    }

    IEnumerator ResetIsAttacked()
    {
        yield return new WaitForSeconds(0.5f); // Adjust this cooldown duration as needed
        isAttacked = false;
    }

    void Die ()
    {
        OnPlayerDeath.Invoke();
    }
}
