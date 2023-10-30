using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingAttack : MonoBehaviour
{

    [Header("Sword Properties")]
    [SerializeField] private Animator swordAnimator;
    [SerializeField] private BoxCollider2D hitBox;

    void Awake() {
        hitBox.enabled = false;
    }

    void EnableAttack() {
        // enable hitbox
        hitBox.enabled = true;
    }

    void DisableAttack() {
        Debug.Log("Disable");
        // set animator paramters isAttacking to False
        swordAnimator.SetBool("isAttacking", false);
        // disable hitbox
        hitBox.enabled = false;
    }

    // instantiate damage here for more control

    // initiate damage calculations here for more damage percentage control

    // and initiate critical strike percentage here for damage mulitplier

    // then we calculate the critical change depending on the crit chance of the weapon attributes

    // we'll prolly need to make 
}
