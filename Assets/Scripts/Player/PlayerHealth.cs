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
            Debug.Log("Taking damage: " + damage);
            health -= damage;
            Debug.Log("Current health: " + health);
            feedbacks.PlayFeedbacks();
            OnPlayerHit.Invoke();
            healthBar.DrawHearts(health, maxHealth);
            PlayFeedbacks();
            isAttacked = true;
            StartCoroutine(ResetIsAttacked());
        }
    }

    public void PlayFeedbacks()
    {
        if (health <= 2)
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
