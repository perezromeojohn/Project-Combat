using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private StatUpgrade[] stats;
    [SerializeField] private StatUpgrade chosenStat;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private SpriteRenderer statIcon; // a gameobject with a sprite renderer to show the stat icon
    [SerializeField] private TextMeshPro statNameText; // a gameobject with a text mesh pro to show the stat name

    private BoxCollider2D boxCollider2D;
    public MMF_Player feedbacks;
    public bool isCollected = false;

    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        chosenStat = stats[Random.Range(0, stats.Length)];
        statNameText.text = "+" + chosenStat.statName;
        UpdateStatIcon();
    }

    void UpdateStatIcon()
    {
        if (chosenStat != null && statIcon != null)
        {
            string iconPath = "Stat Icons/" + chosenStat.statDisplayName;
            Sprite newSprite = Resources.Load<Sprite>(iconPath);
            if (newSprite != null)
            {
                statIcon.sprite = newSprite;
            }
            else
            {
                Debug.LogWarning("Sprite not found for stat: " + chosenStat.statName);
            }
        }
        else
        {
            Debug.LogWarning("Chosen stat or stat icon is null");
        }
    }

    void IncreaseStats()
    {
        // get chosen stat Stat Name and look for it in the player stats
        // if found, increase by 10%
        for (int i = 0; i < stats.Length; i++)
        {
            if (chosenStat.statName == stats[i].statName)
            {
                if (chosenStat.statName == "Movement Speed")
                {
                    playerStats.movementSpeed += playerStats.movementSpeed * chosenStat.baseIncreaseAmount;
                }
                else if (chosenStat.statName == "Magnet Range")
                {
                    playerStats.magnetRange += playerStats.magnetRange * chosenStat.baseIncreaseAmount;
                }
                else if (chosenStat.statName == "Max Health")
                {
                    playerStats.maxHealth += playerStats.maxHealth * chosenStat.baseIncreaseAmount;
                }
                else if (chosenStat.statName == "Luck")
                {
                    playerStats.luck += playerStats.luck * chosenStat.baseIncreaseAmount;
                }
                else if (chosenStat.statName == "Physical Damage")
                {
                    playerStats.physicalDamage += playerStats.physicalDamage * chosenStat.baseIncreaseAmount;
                }
                else if (chosenStat.statName == "Crit Chance")
                {
                    playerStats.critChance += playerStats.critChance * chosenStat.baseIncreaseAmount;
                }
                else if (chosenStat.statName == "Roll Cooldown")
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
