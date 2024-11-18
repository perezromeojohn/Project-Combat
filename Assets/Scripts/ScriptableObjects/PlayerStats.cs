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
    public float physicalDamage;
    public float luck;
    public float critChance;
    public float critDamage;
}
