using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunflowerTurret : MonoBehaviour
{
    private PerkManager perkManager;
    public Perks perk;
    private SkillCooldown cooldownScript;

    [Header("Sunflower Turret Components")]
    private int projectileAmount = 1;
    private float projectileDamage = 10;
    private float turretLifetime = 10f;
    private float projectileInterval = 5f;
    private float cooldown = 0;
    private float calculatedDamage;
    public float damageMultiplier = 1.2f;
    public GameObject turretPrefab;

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
                Debug.Log("Turret not found");
            }
        }
    }

    void ActivateSkill(int skillLevel)
    {
        // Calculate the damage multiplier based on the skill level
        float damageMultiplier = 1f + (0.2f * (skillLevel - 1));

        // Calculate the damage using the base damage and the calculated damage multiplier
        calculatedDamage = projectileDamage * damageMultiplier;

        // Add the player's physical damage to the calculated damage
        float playerPhysicalDamage = perkManager.inGamePlayerStats.physicalDamage;
        calculatedDamage += playerPhysicalDamage;

        // Calculate critical hit
        float critChance = perkManager.inGamePlayerStats.critChance;
        float randomValue = Random.value * 100;
        if (randomValue <= critChance)
        {
            calculatedDamage *= 2;
            Debug.Log("Critical hit!");
        }

        // Additional logic for other properties based on the skill level
        switch(skillLevel)
        {
            case 2:
                projectileAmount = 2;
                break;
            case 3:
                projectileAmount = 3;
                projectileInterval = 3f;
                break;
            case 4:
                projectileAmount = 4;
                projectileInterval = 2f;
                break;
            case 5:
                projectileAmount = 7;
                projectileInterval = 1f;
                turretLifetime = 8f;
                break;
        }
        CastSkill(Mathf.Floor(calculatedDamage), projectileAmount, turretLifetime);
        calculatedDamage = 0;
    }

    void CastSkill(float damage, int turretAmount, float lifetime)
    {
        // instantiate the turret prefab in this position
        GameObject turret = Instantiate(turretPrefab, transform.position, Quaternion.identity);
        turret.GetComponent<Turret>().projectileCount = turretAmount;
        turret.GetComponent<Turret>().turretLifeTime = lifetime;
        turret.GetComponent<Turret>().projectileInterval = projectileInterval;
        turret.GetComponent<Turret>().damage = damage;
    }
}
