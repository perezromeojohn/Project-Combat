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
    public GameObject projectilePrefab;
    private List<Collider2D> hitEnemies = new List<Collider2D>();

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

    void CastSkill(float level, float damage, float projectileCount)
    {
        int centerIndex = (int)(projectileCount / 2); // Index of the center projectile
        float angleSpread = 12f; // Angle spread in degrees

        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = cursorPos - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angleSpread *= Mathf.Deg2Rad; // Convert angleSpread to radians

        for (int i = 0; i < projectileCount; i++)
        {
            GameObject fireball = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            GameObject fireballSpriteRenderer = fireball.transform.GetChild(0).gameObject;
            Projectile fireballProjectile = fireball.GetComponent<Projectile>();
            fireballProjectile.damage = damage;

            // Calculate the direction of the fireball based on its index
            if (i == centerIndex)
            {
                fireballSpriteRenderer.transform.rotation = Quaternion.Euler(0, 0, angle - 45);
                LeanTween.move(fireball, (Vector2)transform.position + direction.normalized * 3, 2f)
                    .setEaseLinear()
                    .setOnComplete(() => Destroy(fireball));
            }
            else
            {
                float offsetAngle = (i - centerIndex) * angleSpread;
                Vector2 rotatedDirection = Quaternion.Euler(0, 0, angle + offsetAngle * Mathf.Rad2Deg) * Vector2.right;
                fireballSpriteRenderer.transform.rotation = Quaternion.Euler(0, 0, angle + offsetAngle * Mathf.Rad2Deg - 45);
                LeanTween.move(fireball, (Vector2)transform.position + rotatedDirection.normalized * 3, 2f)
                    .setEaseLinear()
                    .setOnComplete(() => Destroy(fireball));
            }
        }
    }
}
