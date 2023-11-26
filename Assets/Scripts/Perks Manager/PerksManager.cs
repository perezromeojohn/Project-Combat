using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using UnityEditor;

public class PerksManager : MonoBehaviour
{
    public GameObject levelUpWindow;
    public List<Perks> perksList;
    public List<GameObject> buttons;
    
    public void OpenLevelUpWindow()
    {
        UpdateButtons();
        levelUpWindow.SetActive(true);
        // also stop the game time
        Time.timeScale = 0;
        Debug.Log("Level up window opened");
    }

    public void UpdateButtons()
    {
        if (perksList.Count == 0 || buttons.Count == 0)
        {
            Debug.LogWarning("Perks list or buttons not assigned.");
            return;
        }

        int perksCount = Mathf.Min(buttons.Count, 3); // Limit perks to the number of available buttons or 3, whichever is smaller

        Shuffle(perksList);

        for (int i = 0; i < perksCount; i++)
        {
            Perks perk = perksList[i]; // Retrieve the perk from the list
            // get the perkName and then set the button's name to the perkName
            buttons[i].name = perksList[i].perkName;
            TextMeshProUGUI title = buttons[i].transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI description = buttons[i].transform.Find("Description").GetComponent<TextMeshProUGUI>();
            Image icon = buttons[i].transform.Find("Image").GetComponent<Image>();
            // Load the icon based on your asset structure
            string iconPath = "Assets/GUI/Skill Icons/Skills/" + perk.perkName + ".png"; // Adjust the path accordingly
            if (File.Exists(iconPath))
            {
                icon.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(iconPath);
            }
            else
            {
                Debug.LogWarning("Icon not found for perk " + perk.perkName);
            }

            // Assign values to UI elements
            title.text = perk.perkName;
            description.text = perk.perkDescription;
        }
    }

    // Utility method to shuffle a list
    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randIndex];
            list[randIndex] = temp;
        }
    }
}
