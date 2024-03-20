using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public float experienceMultiplier = 1.3f;
    public int currentLevel = 0;
    public int currentExperienceRequirement = 13;
    public int playerExperience = 0;

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
        currentExperienceRequirement = Mathf.RoundToInt(currentExperienceRequirement * experienceMultiplier);
        playerExperience = 0;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    void TweenProgressBar()
    {
        levelText.text = "World Level: " + currentLevel;
        var experiencePercentage = (float)playerExperience / (float)currentExperienceRequirement;
        LeanTween.scale(experienceBar.gameObject, new Vector3(experiencePercentage, 1f, 1f), .5f)
            .setEase(LeanTweenType.easeInOutQuart);
    }
}

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelManager levelManager = (LevelManager)target;
        if (GUILayout.Button("Level Up"))
        {
            levelManager.IncrementExperience(100);
        }

        // create a reset button to reset the level and experience
        if (GUILayout.Button("Reset Level"))
        {
            levelManager.currentLevel = 1;
            levelManager.currentExperienceRequirement = 13;
            levelManager.playerExperience = 0;
        }
    }
}

