using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private StatUpgrade[] stats;
    [SerializeField] private StatUpgrade chosenStat;
    [SerializeField] private PlayerStats playerStats;

    private BoxCollider2D boxCollider2D;
    public MMF_Player feedbacks;
    public bool isCollected = false;

    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        chosenStat = stats[Random.Range(0, stats.Length)];
    }

    void IncreaseStats()
    {
        // get chosen stat Stat Name and look for it in the player stats
        // if found, increase by 10%
        for (int i = 0; i < stats.Length; i++)
        {
            if (chosenStat.statName == stats[i].statName)
            {
                if (chosenStat.statName == "movementSpeed")
                {
                    playerStats.movementSpeed += playerStats.movementSpeed * chosenStat.baseIncreaseAmount;
                }
                else if (chosenStat.statName == "magnetRange")
                {
                    playerStats.magnetRange += playerStats.magnetRange * chosenStat.baseIncreaseAmount;
                }
                else if (chosenStat.statName == "damage")
                {
                    playerStats.damage += playerStats.damage * chosenStat.baseIncreaseAmount;
                }
                else if (chosenStat.statName == "critChance")
                {
                    playerStats.critChance += playerStats.critChance * chosenStat.baseIncreaseAmount;
                }
                else if (chosenStat.statName == "critDamage")
                {
                    playerStats.critDamage += playerStats.critDamage * chosenStat.baseIncreaseAmount;
                }
                else if (chosenStat.statName == "attackSpeed")
                {
                    playerStats.attackSpeed += playerStats.attackSpeed * chosenStat.baseIncreaseAmount;
                }
                else if (chosenStat.statName == "knockbackStrength")
                {
                    playerStats.knockbackStrength += playerStats.knockbackStrength * chosenStat.baseIncreaseAmount;
                }
                else if (chosenStat.statName == "rollSpeed")
                {
                    playerStats.rollSpeed += playerStats.rollSpeed * chosenStat.baseIncreaseAmount;
                }
                else if (chosenStat.statName == "rollCooldown")
                {
                    playerStats.rollCooldown += playerStats.rollCooldown * chosenStat.baseIncreaseAmount;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;
            feedbacks.PlayFeedbacks();
            IncreaseStats();
        }
    }
}
