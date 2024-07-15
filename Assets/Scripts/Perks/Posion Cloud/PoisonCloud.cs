using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloud : MonoBehaviour
{
    private PerkManager perkManager;
    public Perks perk;
    private SkillCooldown cooldownScript;
    public PoisonSmog poisonSmog;
    public PoisionSmogCollision poisionSmogCollision;

    [Header("Poison Smog Components")]
    private float skillDamage = 3;
    private float cooldown = 0;
    private float duration = 3;
    private float calculatedDamage;
    public float damageMultiplier = 1.8f;
    public float flameThrowerRange = 1f;

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
                Debug.Log("Eggbomb not found");
            }
        }
    }

    void ActivateSkill(int skillLevel)
    {
        // Calculate the damage multiplier based on the skill level
        float damageMultiplier = 1f + (0.2f * (skillLevel - 1));

        // Calculate the damage using the base damage and the calculated damage multiplier
        calculatedDamage = skillDamage * damageMultiplier;

        // Additional logic for other properties based on the skill level
        switch(skillLevel)
        {
            case 2:
                break;
            case 3:
                flameThrowerRange = 1.5f;
                break;
            case 4:
                break;
            case 5:
                flameThrowerRange = 2f;
                duration = 5;
                break;
        }
        CastSkill(Mathf.Floor(calculatedDamage), flameThrowerRange, duration);
        calculatedDamage = 0;
    }

    void CastSkill(float damage, float range, float flameThrowerDuration)
    {
        poisionSmogCollision.damage = damage;
        poisonSmog.flameThrowerRange = range;
        poisonSmog.duration = flameThrowerDuration;
        poisonSmog.FireFlameThrower();
    }
}
