using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Player Stats")]
    public float movementSpeed;
    public float health;
    public float maxHealth;
    [Header("Player Attacking")]
    public float damage;
    public float critChance;
    public float critMultiplier;
    public float critDamage;
    public float attackSpeed;
    [Header("Player Mobility")]
    public float rollSpeed;
    public float rollCooldown;
    public float blinkAmount;
    public float blinkCooldown;
}
