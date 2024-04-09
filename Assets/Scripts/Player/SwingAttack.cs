using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEditor;
using MoreMountains.FeedbacksForThirdParty;
using Unity.VisualScripting;

public class SwingAttack : MonoBehaviour
{
    [Header("MM Feedbacks")]
    public MMF_Player cameraImpulse;
    private MMF_FloatingText floatingText;
    private MMF_CinemachineImpulse impulse;
    private bool isPlaying = false;

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
        impulse = cameraImpulse.GetFeedbackOfType<MMF_CinemachineImpulse>();
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
        // Debug.Log(other.name);
        if (!hitEnemies.Contains(other)) {
            // if tag is enemy
            if (other.gameObject.CompareTag("Enemy")) {
                hitEnemies.Add(other);
                // Debug.Log(other.name);
                damage = playerStats.damage;
                other.GetComponent<Behavior>().isAttacked = true;
                other.GetComponent<Behavior>().damageTaken = damage;
                StartCoroutine(Feedbacks());
            }
        }
    }
    // I need to set URP to this project

    private IEnumerator Feedbacks()
    {
        if (!isPlaying)
        {
            cameraImpulse.PlayFeedbacks();
            isPlaying = true;
            yield return new WaitForSeconds(0.3f);
            isPlaying = false;
        }
    }
}
