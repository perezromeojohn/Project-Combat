using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Player Stats")]
    public float movementSpeed;
    public float maxHealth;
    public float magnetRange;
    [Header("Player Attacking")]
    public float physicalDamage;
    public float magicDamage;
    public float critChance;
    public float critDamage;
    public float knockbackStrength;
    [Header("Player Mobility")]
    public float rollSpeed;
    public float rollCooldown;
}
