using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public PerkManager perkManager;
    public float experienceMultiplier = 1.3f;
    public int currentLevel = 0;
    public int currentExperienceRequirement = 13;
    public int playerExperience = 0;
    public AudioSource levelUpSound;

    [Header("GUI Stuff")]
    public TextMeshProUGUI levelText;
    public Image experienceBar;

    void Start()
    {
        TweenProgressBar();
    }

    public void IncrementExperience(int experienceGained)
    {
        playerExperience += experienceGained;
        if (playerExperience >= currentExperienceRequirement)
        {
            LevelUp();
        }
        TweenProgressBar();
    }

    private void LevelUp()
    {
        currentLevel++;
        levelUpSound.Play();
        currentExperienceRequirement = Mathf.RoundToInt(currentExperienceRequirement * experienceMultiplier);
        playerExperience = 0;
        perkManager.ShowAllPerkButtons();
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    void TweenProgressBar()
    {
        levelText.text = "World Level: " + currentLevel;
        var experiencePercentage = (float)playerExperience / (float)currentExperienceRequirement;
        experienceBar.transform.localScale = new Vector3(experiencePercentage, 1, 1);
    }
}

