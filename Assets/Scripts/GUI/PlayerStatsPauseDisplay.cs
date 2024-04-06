using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Reflection;

public class PlayerStatsPauseDisplay : MonoBehaviour
{
    public PlayerStats playerStats;
    public GameObject[] playerStatsFrame;

    void ApplyStats()
    {
        for (int i = 0; i < playerStatsFrame.Length; i++)
        {
            GameObject valueObject = playerStatsFrame[i].transform.Find("Value").gameObject;
            TextMeshProUGUI valueText = valueObject.GetComponent<TextMeshProUGUI>();

            if (valueText != null)
            {
                switch (i)
                {
                    case 0:
                        valueText.text = playerStats.movementSpeed.ToString();
                        break;
                    case 1:
                        valueText.text = playerStats.maxHealth.ToString();
                        break;
                    case 2:
                        valueText.text = playerStats.magnetRange.ToString();
                        break;
                    case 3:
                        valueText.text = playerStats.damage.ToString();
                        break;
                    case 4:
                        valueText.text = playerStats.critChance.ToString() + "%";
                        break;
                    case 5:
                        valueText.text = playerStats.critDamage.ToString() + "%";
                        break;
                    case 6:
                        valueText.text = playerStats.attackSpeed.ToString() + "%";
                        break;
                    case 7:
                        valueText.text = playerStats.knockbackStrength.ToString();
                        break;
                    case 8:
                        valueText.text = playerStats.dashCharge.ToString();
                        break;
                    case 9:
                        valueText.text = playerStats.rollSpeed.ToString();
                        break;
                    case 10:
                        valueText.text = playerStats.rollCooldown.ToString();
                        break;
                    // Add cases for other stats as needed
                }
            }
        }
    }

    void Update()
    {
        ApplyStats();
    }
}
