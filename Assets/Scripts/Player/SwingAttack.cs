using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEditor;

public class SwingAttack : MonoBehaviour
{
    [Header("MM Feedbacks")]
    public MMF_Player feedbacks;
    private MMF_FloatingText floatingText;

    [Header("Player Stats")]
    public PlayerStats playerStats;
    public Rigidbody2D rb;
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

    void DamageNumbers(Transform hit)
    {
        floatingText = feedbacks.GetFeedbackOfType<MMF_FloatingText>();
        floatingText.PositionMode = MMF_FloatingText.PositionModes.TargetTransform;
        floatingText.TargetTransform = hit;
        floatingText.Value = damage.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.name);
        if (!hitEnemies.Contains(other)) {
            // if tag is enemy
            if (other.gameObject.CompareTag("Enemy")) {
                hitEnemies.Add(other);
                Debug.Log(other.name);
                damage = playerStats.damage;
                other.GetComponent<Behavior>().isAttacked = true;
                other.GetComponent<Behavior>().damageTaken = damage;
                DamageNumbers(other.transform);
                feedbacks.PlayFeedbacks();
            }
        }
    }
}
