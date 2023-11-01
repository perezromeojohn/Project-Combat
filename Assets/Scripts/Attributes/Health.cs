using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void getHealth() {
        Debug.Log(currentHealth);
    }

    void setHealth(float health) {
        currentHealth = health;
    }
}
