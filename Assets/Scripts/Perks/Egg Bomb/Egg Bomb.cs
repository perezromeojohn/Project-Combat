using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBomb : MonoBehaviour
{
    private PerkManager perkManager;
    public Perks perk;
    private SkillCooldown cooldownScript;

    [Header("Egg Bomb Components")]
    private float skillProjectile = 1;
    private float skillDamage = 10;
    private float cooldown = 0;
    private float calculatedDamage;
    public float damageMultiplier = 1.2f;
    public GameObject projectilePrefab;

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
        // Calculate the damage multiplier based on the skill level
        float damageMultiplier = 1f + (0.2f * (skillLevel - 1));

        // Calculate the damage using the base damage and the calculated damage multiplier
        calculatedDamage = skillDamage * damageMultiplier;

        // Additional logic for other properties based on the skill level
        switch(skillLevel)
        {
            case 2:
                skillProjectile = 1;
                break;
            case 3:
                skillProjectile = 3;
                break;
            case 4:
                skillProjectile = 2;
                break;
            case 5:
                skillProjectile = 5;
                break;
        }
        CastSkill(skillLevel, Mathf.Floor(calculatedDamage), skillProjectile);
        calculatedDamage = 0;
    }

    void CastSkill(int skillLevel, float damage, float projectileCount)
    {
        for (int i = 0; i < projectileCount; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.transform.position = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), transform.position.z);
        }
    }
}
