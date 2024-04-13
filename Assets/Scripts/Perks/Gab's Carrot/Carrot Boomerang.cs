using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotBoomerang : MonoBehaviour
{
    private PerkManager perkManager;
    public Perks perk;

    [Header("Egg Bomb Components")]
    private int carrotIndex = 0;
    private float skillDamage = 15;
    private float cooldown = 0;
    private float calculatedDamage;
    public float damageMultiplier = 1.2f;
    private float rotationSpeed = 180; // 45 degrees per second
    public GameObject[] carrots;

    void Start()
    {
        perkManager = GameObject.Find("PerksManager").GetComponent<PerkManager>();
        cooldown = perk.perkCooldown;

        for (int i = 0; i < carrots.Length; i++)
        {
            carrots[i].SetActive(false);
            Debug.Log("Carrot " + i + " is inactive");
        }
    }

    void Update()
    {
        if (perkManager.playerPerks.ContainsKey(perk.perkName)) 
        {
            {
                int perkLevel = perkManager.playerPerks[perk.perkName];
                switch(perkLevel)
                {
                    case 2:
                        rotationSpeed = 220;
                        break;
                    case 3:
                        carrotIndex = 1;
                        break;
                    case 4:
                        rotationSpeed = 290;
                        break;
                    case 5:
                        rotationSpeed = 360;
                        break;
                }
            }
        }

        for (int i = 0; i < carrots.Length; i++)
        // decrement the i by 1 because the array index starts at 0
        {
            if (carrotIndex == i)
            {
                carrots[i].SetActive(true);
                carrots[i].GetComponent<CarrotProjectile>().damage = skillDamage;
                Debug.Log("Carrot " + i + " is active");
            }
        }
        transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
    }
}
