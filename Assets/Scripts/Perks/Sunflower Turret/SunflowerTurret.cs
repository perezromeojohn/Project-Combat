using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunflowerTurret : MonoBehaviour
{
    private PerkManager perkManager;
    public Perks perk;
    private SkillCooldown cooldownScript;

    [Header("Sunflower Turret Components")]
    private int turretAmount = 1;
    private float beamDamage = 10;
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
        calculatedDamage = beamDamage * damageMultiplier;

        // Additional logic for other properties based on the skill level
        switch(skillLevel)
        {
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
        }
        CastSkill(Mathf.Floor(calculatedDamage));
        calculatedDamage = 0;
    }

    void CastSkill(float damage)
    {
        // instantiate the turret prefab in this position
        GameObject turret = Instantiate(turretPrefab, transform.position, Quaternion.identity);
        
    }
}
