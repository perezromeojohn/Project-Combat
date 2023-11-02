using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingAttack : MonoBehaviour
{

    [Header("Player Stats")]
    public PlayerStats playerStats;
    [Header("Sword Properties")]
    [SerializeField] private Animator swordAnimator;
    [SerializeField] private CircleCollider2D hitBox;
    private List<Collider2D> hitEnemies = new List<Collider2D>();
    private float damage = 0f;

    void Awake() {
        hitBox.enabled = false;
    }

    void EnableAttack() {
        // enable hitbox
        hitBox.enabled = true;
    }

    void DisableAttack() {
        swordAnimator.SetBool("isAttacking", false);
        // disable hitbox
        hitBox.enabled = false;
        // clear list of hit enemies
        hitEnemies.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!hitEnemies.Contains(other)) {
            hitEnemies.Add(other);
            Debug.Log(other.name);
            other.GetComponent<EnemyBehaviour>().isAttacked = true;
            damage = playerStats.damage;
            other.GetComponent<EnemyBehaviour>().damageTaken = damage;
        }
    }
}
