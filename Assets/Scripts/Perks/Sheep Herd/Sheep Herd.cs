using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepHerd : MonoBehaviour
{
     private PerkManager perkManager;
    public Perks perk;
    private SkillCooldown cooldownScript;

    [Header("Sheep Herd Components")]
    private int sheepAmount = 1;
    private float skillDamage = 10;
    private float cooldown = 0;
    private float calculatedDamage;
    public float damageMultiplier = 1.2f;
    public GameObject sheepHerd;

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
                Debug.Log("Sheep Herd not found");
            }
        }
    }

    void ActivateSkill(int skillLevel)
    {
        float damageMultiplier = 1f + (0.2f * (skillLevel - 1));
        calculatedDamage = skillDamage * damageMultiplier;
        switch(skillLevel)
        {
            case 2:
                sheepAmount = 2;
                break;
            case 3:
                sheepAmount = 3;
                break;
            case 4:
                sheepAmount = 4;
                break;
            case 5:
                sheepAmount = 5;
                break;
        }
        CastSkill(Mathf.Floor(calculatedDamage), sheepAmount);
        calculatedDamage = 0;
    }

    void CastSkill(float damage, int projectileCount)
    {
        StartCoroutine(SpawnProjectiles(damage, projectileCount));
    }

    IEnumerator SpawnProjectiles(float damage, int projectileCount)
    {
        GameObject projectile = Instantiate(sheepHerd, transform.position, Quaternion.identity);

        for (int i = 0; i < projectile.transform.childCount; i++)
        {
            projectile.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < projectile.transform.childCount; i++)
        {
            if (i < projectileCount)
            {
                projectile.transform.GetChild(i).gameObject.SetActive(true);
                projectile.transform.GetChild(i).GetComponent<Sheep>().damage = damage;
                // set projectile scale to 0, 1, 0
                projectile.transform.GetChild(i).localScale = new Vector3(0, 0, 0);
                // leanTween scale them to 1, 1, 1 by .1f
                LeanTween.scale(projectile.transform.GetChild(i).gameObject, new Vector3(1, 1, 1), 0.2f);
            }
            else
            {
                projectile.transform.GetChild(i).gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
