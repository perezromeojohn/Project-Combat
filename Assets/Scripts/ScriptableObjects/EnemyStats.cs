using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    public string enemyName;
    public float movementSpeed;
    public float minDistanceToPlayer;
    public float damage;
    public float health;
    public float maxHealth;
    public Color spriteColor;
}
