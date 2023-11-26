#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int soulsCollected = 0;
    [SerializeField] private int totalSoulsCollected;
    [SerializeField] private int worldLevel = 1;
    public int soulsRequiredForLevelUp = 15;
    private readonly float levelMultiplier = 1.2f; // Adjust this multiplier as needed
    public MMFeedbacks levelUpFeedback;
    public PerksManager perksManager;

    public GameObject progressBar;
    // get textmeshpro
    public TextMeshProUGUI levelText;

    // can you make me a button that will add 1 to soulsCollected?
    

    // Update is called once per frame
    void Update()
    {
        // Check if the player has collected enough souls to level up
        if (soulsCollected > soulsRequiredForLevelUp)
        {
            LevelUp();
        }

        UpdateProgressBarUI();
    }

    public void IncrementSoulsCollected()
    {
        soulsCollected++;
    }
    

    void LevelUp()
    {
        // Open the level up window
        perksManager.OpenLevelUpWindow();
        // Print "Level up" to the console
        Debug.Log("Level up!");

        // Add the souls collected to the total
        totalSoulsCollected += soulsCollected;

        // level up the world level
        worldLevel++;

        // Calculate the new amount of souls required for the next level
        soulsRequiredForLevelUp = Mathf.RoundToInt(soulsRequiredForLevelUp * levelMultiplier);

        // Reset souls collected for the next level
        soulsCollected = 0;
        // convert to string and set textmeshpro to World Level concatenate with the converted string worldLevel
        levelText.text = worldLevel.ToString();
        

        // Update UI or provide feedback
        // ...

        // Optionally, you can adjust other aspects of the game, like enemy difficulty
        // ...

        // Log the total souls collected and the new required souls for debugging or analytics
        Debug.Log("Total Souls Collected: " + totalSoulsCollected);
        Debug.Log("Souls Required for Next Level: " + soulsRequiredForLevelUp);
    }

    // void UpdateUI()
    // {
    //     // Update the level text
    //     levelText.text = "Level: " + totalSoulsCollected;

    //     // Update the progress bar
    //     progressBar.value = (float)soulsCollected / soulsRequiredForLevelUp;
    // }

    #if UNITY_EDITOR
    [CustomEditor(typeof(LevelManager))]
    public class LevelManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            LevelManager myScript = (LevelManager)target;

            if (GUILayout.Button("Add Souls"))
            {
                myScript.AddSouls(1); // You can adjust the amount as needed
            }

            if (GUILayout.Button("Level Up"))
            {
                myScript.LevelUp();
            }
        }
    }
    #endif


    public void AddSouls(int amount)
    {
        soulsCollected += amount;
        levelUpFeedback.PlayFeedbacks();
        Debug.Log("Added " + amount + " souls. Total souls collected: " + soulsCollected);
    }

    void UpdateProgressBarUI()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        float progress = (float)soulsCollected / soulsRequiredForLevelUp;

        LeanTween.scaleX(progressBar, progress, .1f).setEaseLinear();
    }

}
