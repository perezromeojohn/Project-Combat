using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheatMissile : MonoBehaviour
{
    private PerkManager perkManager;
    public Perks perk;
    private SkillCooldown cooldownScript;

    [Header("Wheat Missile Components")]
    private int skillProjectile = 5;
    private float skillDamage = 10;
    private float cooldown = 0;
    private float calculatedDamage;
    public float damageMultiplier = 1.2f;
    public GameObject wheatProjectilePrefab;
    public float delayBetweenProjectiles = 0.2f;
    public float range = .3f;

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
                Debug.Log("Wheat Missile not found");
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
                range = .5f;
                break;
            case 3:
                skillProjectile = 7;
                break;
            case 4:
                range = .7f;
                break;
            case 5:
                skillProjectile = 10;
                break;
        }
        CastSkill(Mathf.Floor(calculatedDamage), skillProjectile, range);
        calculatedDamage = 0;
    }

    void CastSkill(float damage, int projectileCount, float projRange)
    {
        StartCoroutine(SpawnProjectiles(damage, projectileCount, projRange));
    }

    IEnumerator SpawnProjectiles(float damage, int projectileCount, float projRange)
    {

        for (int i = 0; i < projectileCount; i++)
        {
            // Vector3 spawnPosition = transform.position + new Vector3((col - numRows / 2) * spacing, (row - numRows / 2) * spacing, 0);
            // random spawn based on the range
            Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-projRange, projRange), Random.Range(-projRange, projRange), 0);
            GameObject projectile = Instantiate(wheatProjectilePrefab, spawnPosition, Quaternion.identity);
            
            projectile.GetComponent<WheatProjectile>().damage = damage;

            yield return new WaitForSeconds(delayBetweenProjectiles);
        }
    }
}
