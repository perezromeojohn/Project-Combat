using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Events;
using Unity.Mathematics;
using MoreMountains.Feedbacks;

public class PlayerHealth : MonoBehaviour
{
    // get character playerStats
    public PlayerStats playerStats;
    public Collider2D playerCollider;
    public HealthBar healthBar;
    private float maxHealth;
    private float health;
    public MMF_Player feedbacks;
    private MMF_CameraOrthographicSize orthographicSize;
    private MMF_TimescaleModifier timescaleModifier;
    public MMF_Player healFeedbacks;

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
        orthographicSize = feedbacks.GetFeedbackOfType<MMF_CameraOrthographicSize>();
        timescaleModifier = feedbacks.GetFeedbackOfType<MMF_TimescaleModifier>();
        orthographicSize.Active = false;
        timescaleModifier.Active = false;
    }

    public void TakeDamage(float damage)
    {
        if (!isAttacked)
        {
            // Debug.Log("Taking damage: " + damage);
            health -= damage;
            // Debug.Log("Current health: " + health);
            feedbacks.PlayFeedbacks();
            OnPlayerHit.Invoke();
            healthBar.DrawHearts(health, maxHealth);
            PlayFeedbacks();
            isAttacked = true;
            StartCoroutine(ResetIsAttacked());
            // Debug Log all enemies that is in 1f range using gameObject.FindGameObjectsWithTag("Enemy")
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                if (Vector2.Distance(enemy.transform.position, transform.position) < .3f)
                {
                    enemy.GetComponent<Behavior>().isAttacked = true;
                }
            }
        }
    }

    public void HealDamage(float heal)
    {
        health += heal;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        healthBar.DrawHearts(health, maxHealth);
        healFeedbacks.PlayFeedbacks();
        // Debug.Log("Healing: " + heal);
    }

    public void PlayFeedbacks()
    {
        if (health <= 1)
        {
            orthographicSize.Active = true;
            timescaleModifier.Active = true;
        }
        else
        {
            orthographicSize.Active = false;
            timescaleModifier.Active = false;
        }
        feedbacks.PlayFeedbacks();
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
