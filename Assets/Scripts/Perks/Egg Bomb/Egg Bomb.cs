using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBomb : MonoBehaviour
{
    private PerkManager perkManager;
    public Perks perk;
    private SkillCooldown cooldownScript;

    [Header("Egg Bomb Components")]
    private int skillProjectile = 1;
    private float skillDamage = 10;
    private float cooldown = 0;
    private float calculatedDamage;
    public float damageMultiplier = 1.2f;
    public GameObject projectilePrefab;
    public float delayBetweenProjectiles = 0.3f;
    public float range = 0.5f;

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
                range = 0.75f;
                break;
            case 3:
                skillProjectile = 2;
                break;
            case 4:
                skillProjectile = 3;
                range = 1f;
                break;
            case 5:
                skillProjectile = 4;
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
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            EggLob eggLob = projectile.GetComponent<EggLob>();
            EggAnim eggAnim = projectile.transform.GetChild(0).GetComponent<EggAnim>();

            eggAnim.damage = damage;
            eggLob.defaultRandomRange = projRange;

            yield return new WaitForSeconds(delayBetweenProjectiles);
        }
    }
}
