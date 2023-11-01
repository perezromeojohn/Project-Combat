using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingAttack : MonoBehaviour
{

    [Header("Sword Properties")]
    [SerializeField] private Animator swordAnimator;
    [SerializeField] private CircleCollider2D hitBox;

    void Awake() {
        hitBox.enabled = false;
    }

    void EnableAttack() {
        // enable hitbox
        hitBox.enabled = true;
    }

    void DisableAttack() {
        // Debug.Log("Disable");
        // set animator paramters isAttacking to False
        swordAnimator.SetBool("isAttacking", false);
        // disable hitbox
        hitBox.enabled = false;
    }

    // detect if CircleCollider2D overlaps with enemy collider
    // if so, print the collider's name
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.name);
    }
}
