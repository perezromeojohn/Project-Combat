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
    public float maxHealth;
    public Vector3 healthBarUIOffset;
    [Header("Misc")]
    public float experienceValue;
}
