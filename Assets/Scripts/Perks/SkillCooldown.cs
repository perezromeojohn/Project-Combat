using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCooldown : MonoBehaviour
{
    private float cooldownDuration = 0f;
    public float cooldownTimer = 0f;
    public bool isOnCooldown = false;

    public void StartCooldown(float duration)
    {
        cooldownDuration = duration;
        cooldownTimer = 0f;
        isOnCooldown = true;
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer >= cooldownDuration)
        {
            Debug.Log("Cooldown finished!");
            cooldownTimer = 0f;
            isOnCooldown = false;
        }
    }
}
