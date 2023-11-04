using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class SwingAttack : MonoBehaviour
{
    [Header("MM Feedbacks")]
    public MMFeedbacks feedbacks;

    [Header("Player Stats")]
    public PlayerStats playerStats;
    [Header("Sword Properties")]
    [SerializeField] private Animator swordAnimator;
    [SerializeField] private CapsuleCollider2D hitBox;
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
        // disable hitbox
        hitBox.enabled = false;
        // clear list of hit enemies
        hitEnemies.Clear();
    }

    void ResetAttacking() {
        swordAnimator.SetBool("isAttacking", false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!hitEnemies.Contains(other)) {
            hitEnemies.Add(other);
            Debug.Log(other.name);
            damage = playerStats.damage;
            other.GetComponent<EnemyBehaviour>().isAttacked = true;
            other.GetComponent<EnemyBehaviour>().damageTaken = damage;
            feedbacks.PlayFeedbacks();
        }
    }
}
