using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private PerkManager perkManager;
    public Perks perk;

    [Header("Fireball Components")]
    private float skillProjectile = 1;
    private float skillDamage = 10;
    private float cooldown = 0;
    private float calculatedDamage;
    public float damageMultiplier = 1.2f;
    private SkillCooldown cooldownScript;

    void Start()
    {
        perkManager = GameObject.Find("PerksManager").GetComponent<PerkManager>();
        cooldown = perk.perkCooldown;
        cooldownScript = GetComponent<SkillCooldown>();
    }

    void Update()
    {
        if (cooldownScript != null && cooldownScript.cooldownTimer <= 0 && cooldownScript.isOnCooldown == false)
        {
            if (perkManager.playerPerks.ContainsKey(perk.perkName))
            {
                int perkLevel = perkManager.playerPerks[perk.perkName];
                ActivateSkill(perkLevel);
                cooldownScript.StartCooldown(cooldown); // Start the cooldown after activating the skill
            }
            else
            {
                Debug.Log("Fireball not found");
            }
        }
    }
    
    void ActivateSkill(int skillLevel)
    {
        switch(skillLevel)
        {
            // every level the skill will increase the damage by 20%
            case 1:
                calculatedDamage = skillDamage * damageMultiplier;
                break;
            case 2:
                calculatedDamage = skillDamage * damageMultiplier;
                break;
            case 3:
                calculatedDamage = skillDamage * damageMultiplier;
                skillProjectile = 2;
                break;
            case 4:
                calculatedDamage = skillDamage * damageMultiplier;
                skillProjectile = 3;
                break;
            case 5:
                calculatedDamage = skillDamage * damageMultiplier;
                skillProjectile = 5;

                break;
        }
        CastSkill(skillLevel, Mathf.Floor(calculatedDamage), skillProjectile);
        calculatedDamage = 0;
    }

    void CastSkill(float level, float damage, float projectile)
    {
        Debug.Log("FIRE");
    }
}
