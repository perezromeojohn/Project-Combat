using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthGUI : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Player Stats")]
    public PlayerStats playerStats;
    public GameObject healthBar;
    private float health;
    private float maxHealth;
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
    }

    void UpdateHealthUI()
    {
        float scaleFactor = Mathf.Clamp01(health / maxHealth);
        Vector3 newScale = healthBar.transform.localScale;
        newScale.x = scaleFactor;
        healthBar.transform.localScale = newScale;
    }
}
