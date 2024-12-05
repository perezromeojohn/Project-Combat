using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheatMissile : MonoBehaviour
{
    private PerkManager perkManager;
    public Perks perk;
    private SkillCooldown cooldownScript;

    [Header("Wheat Missile Components")]
    private int skillProjectile = 3;
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
                range = .5f;
                break;
            case 3:
                skillProjectile = 5;
                break;
            case 4:
                range = .7f;
                break;
            case 5:
                skillProjectile = 7;
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
        // Find all enemies within range
        Collider2D[] collidersInRange = Physics2D.OverlapCircleAll(transform.position, projRange);
        List<GameObject> enemiesInRange = new List<GameObject>();
        
        foreach (Collider2D collider in collidersInRange)
        {
            if (collider.CompareTag("Enemy"))
            {
                enemiesInRange.Add(collider.gameObject);
            }
        }

        int enemyCount = Mathf.Min(enemiesInRange.Count, projectileCount);

        // Spawn projectiles on enemies
        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPosition = enemiesInRange[i].transform.position;
            SpawnProjectile(spawnPosition, damage);
            yield return new WaitForSeconds(delayBetweenProjectiles);
        }

        // Spawn remaining projectiles randomly
        for (int i = enemyCount; i < projectileCount; i++)
        {
            Vector3 randomOffset = Random.insideUnitCircle * projRange;
            Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);
            SpawnProjectile(spawnPosition, damage);
            yield return new WaitForSeconds(delayBetweenProjectiles);
        }
    }

    void SpawnProjectile(Vector3 position, float damage)
    {
        GameObject projectile = Instantiate(wheatProjectilePrefab, position, Quaternion.identity);
        projectile.GetComponent<WheatProjectile>().damage = damage;
    }
}
