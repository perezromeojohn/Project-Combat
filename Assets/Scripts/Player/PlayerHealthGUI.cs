using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using TMPro;
public class PlayerHealthGUI : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Player Stats")]
    public PlayerStats playerStats;
    public GameObject healthBar;
    // text meshpro for health
    public TextMeshProUGUI healthText;
    private float health;
    private float maxHealth;

    [Header("MM Feedbacks")]
    public MMFeedbacks feedbacks;
    void Start()
    {
        health = playerStats.health;
        maxHealth = playerStats.maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        UpdateHealthUI();
        feedbacks.PlayFeedbacks();
        if (health <= 0)
        {
            Debug.Log("Player is dead");
            // value set here for the cronies
            // set value to 10
        }
    }

    void UpdateHealthUI()
    {
        float scaleFactor = Mathf.Clamp01(health / maxHealth);
        // to this format health/maxhealth  
        healthText.text = health.ToString() + "/" + maxHealth.ToString();
        Vector3 newScale = healthBar.transform.localScale;
        newScale.x = scaleFactor;
        LeanTween.scale(healthBar, newScale, 0.5f).setEaseOutQuart();
    }
}
