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
                        valueText.text = playerStats.physicalDamage.ToString();
                        break;
                    case 4:
                        valueText.text = playerStats.luck.ToString();
                        break;
                    case 5:
                        valueText.text = playerStats.critChance.ToString() + "%";
                        break;
                    case 6:
                        valueText.text = playerStats.rollCooldown.ToString("F1");
                        break;
                }
            }
        }
    }

    void Update()
    {
        ApplyStats();
    }
}
