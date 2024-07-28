using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillBlade : MonoBehaviour
{
    private PerkManager perkManager;
    public Perks perk;
    private SkillCooldown cooldownScript;

    [Header("Windmill Blade Components")]
    private int projectileCount = 1;
    private float skillDamage = 10;
    private float cooldown = 0;
    private float calculatedDamage;
    public float damageMultiplier = 1.2f;
    public GameObject projectilePrefab;
    public float stayduration = 0.5f;
    public float range = 1f;

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
                Debug.Log("Windmill Blade not found");
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
                projectileCount = 1;
                break;
            case 3:
                projectileCount = 2;
                break;
            case 4:
                projectileCount = 3;
                range = 1f;
                break;
            case 5:
                projectileCount = 4;
                break;
        }
        CastSkill(Mathf.Floor(calculatedDamage), projectileCount, range);
        calculatedDamage = 0;
    }

    void CastSkill(float damage, int projectileCount, float projRange)
    {
        // // instantiate the projectile prefab
        // GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        // // get the blade component from the projectile
        // Blade blade = projectile.GetComponent<Blade>();
        // // set the projectile count and range
        // blade.projectileCount = projectileCount;
        // blade.range = projRange;

        for (int i = 0; i < projectileCount; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Blade blade = projectile.GetComponent<Blade>();
            blade.range = projRange;
            blade.stayDuration = stayduration;
            blade.damage = damage;
        }
    }
}
