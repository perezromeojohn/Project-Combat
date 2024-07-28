using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : MonoBehaviour
{
    private PerkManager perkManager;
    public Perks perk;

    [Header("Fish Missile Components")]
    public float skillProjectile = 3;
    private float skillDamage = 10;
    private float cooldown = 0;
    private float calculatedDamage;
    public float damageMultiplier = 1.2f;
    private float maxDistance = 1f;
    private float maxSpeedMultiplier = 3f;
    public float speedIncreaseRate = 1f;
    public float travelDuration = .2f;
    private SkillCooldown cooldownScript;
    public GameObject projectilePrefab;
    private Rigidbody2D playerRb;

    void Start()
    {
        perkManager = GameObject.Find("PerksManager").GetComponent<PerkManager>();
        cooldown = perk.perkCooldown;
        cooldownScript = GetComponent<SkillCooldown>();
        var player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody2D>();
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
                Debug.Log("Shovel not found");
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
                skillProjectile = 5;
                break;
            case 3:
                skillProjectile = 7;
                break;
            case 4:
                skillProjectile = 9;
                break;
            case 5:
                skillProjectile = 11;
                break;
        }
        CastSkill(Mathf.Floor(calculatedDamage), skillProjectile);
        calculatedDamage = 0;
    }

    void CastSkill(float damage, float projectileCount)
    {
        StartCoroutine(CastSkillWithDelay(damage, projectileCount));
    }

    IEnumerator CastSkillWithDelay(float damage, float projectileCount)
    {
        float angleSpread = 15f * Mathf.Deg2Rad;
        float delayBetweenShovels = 0.02f;

        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = cursorPos - (Vector2)transform.position;
        float baseAngle = Mathf.Atan2(direction.y, direction.x);

        // Calculate the start angle for the leftmost projectile
        float startAngle = baseAngle - (angleSpread * (projectileCount - 1) / 2);

        // Check if the player is moving
        bool isPlayerMoving = playerRb.velocity.magnitude > 0.1f;
        float adjustedMaxDistance = isPlayerMoving ? maxDistance * 1.3f : maxDistance;
        float adjustedAngleSpread = isPlayerMoving ? angleSpread * 0.6f : angleSpread;

        for (int i = 0; i < projectileCount; i++)
        {
            float currentAngle = startAngle + (i * adjustedAngleSpread);
            Vector2 rotatedDirection = new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle));

            GameObject shovel = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            ShovelAttack shovelProjectile = shovel.GetComponent<ShovelAttack>();
            GameObject shovelProjectileSpriteRenderer = shovel.transform.GetChild(0).gameObject;
            shovelProjectile.damage = damage;

            float rotationAngle = currentAngle * Mathf.Rad2Deg - 45;
            shovelProjectileSpriteRenderer.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);

            // Use the same spread angle but adjust the distance
            Vector2 spawnPosition = (Vector2)transform.position + rotatedDirection * adjustedMaxDistance;

            LeanTween.move(shovel, spawnPosition, 1f)
                .setEaseOutCirc()
                .setOnComplete(() => StartCoroutine(ReturnToPlayer(shovel.transform.position, shovel)));

            yield return new WaitForSeconds(delayBetweenShovels);
        }
    }

    IEnumerator ReturnToPlayer(Vector3 from, GameObject shovel)
    {
        float initialSpeed = Vector3.Distance(from, transform.position) / travelDuration;
        float currentSpeed = initialSpeed;
        float elapsedTime = 0f;
        float maxSpeed = initialSpeed * maxSpeedMultiplier;

        while (Vector3.Distance(shovel.transform.position, transform.position) > 0.1f)
        {
            elapsedTime += Time.deltaTime;
            
            if (elapsedTime >= .1f)
            {
                if (currentSpeed < maxSpeed)
                {
                    currentSpeed += speedIncreaseRate;
                }
                elapsedTime = 0f;
            }

            float step = currentSpeed * Time.deltaTime;
            shovel.transform.position = Vector3.MoveTowards(shovel.transform.position, transform.position, step);
            yield return null;
        }

        // Destroy the shovel once it reaches the player
        Destroy(shovel);
    }
}
