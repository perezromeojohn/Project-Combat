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
    public float health;
    public MMF_Player feedbacks;
    private MMF_CameraOrthographicSize orthographicSize;
    private MMF_TimescaleModifier timescaleModifier;
    public MMF_Player healFeedbacks;

    private bool isAttacked = false;

    // events
    public UnityEvent OnPlayerHit;
    public UnityEvent OnPlayerDeath;

    // void Update()
    // {
    //     // if I press N deal 20 damage
    //     if (Input.GetKeyDown(KeyCode.N))
    //     {
    //         TakeDamage(20);
    //     }
    // }

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

        Debug.Log(maxHealth);
    }

    public void UpdateMaxHealth(float addedHealth) // update health as soon as player upgrades health
    {
        maxHealth = playerStats.maxHealth;
        health += addedHealth;
        healthBar.UpdateMaxHealth(addedHealth);
    }

    public void TakeDamage(float damage)
    {
        if (!isAttacked)
        {
            // Debug.Log("Taking damage: " + damage);
            // Debug.Log("Current health: " + health);
            feedbacks.PlayFeedbacks();
            OnPlayerHit.Invoke();
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
            health -= damage;
            healthBar.DrawHearts(health, maxHealth);
            if (health <= 0)
            {
                health = 0;
                Die();
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
        if (health == 0)
        {
            Debug.Log("Player is dedo like deedee");
        }
        else if (health == 2)
        {
            orthographicSize.Active = true;
            timescaleModifier.Active = true;
        }
        else if (health == 1)
        {
            orthographicSize.Active = true;
            timescaleModifier.Active = true;
            
            orthographicSize.RemapOrthographicSizeOne = .7f;
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
