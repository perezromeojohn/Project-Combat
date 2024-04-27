using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;

public class PerkManager : MonoBehaviour
{
    public GameObject[] perkButtons;
    public GameObject perkMenu;
    public TimeManager timeManager;
    public Animator perkMenuAnimator;
    private Animator[] perkAnimators;
    private Button[] perkButtonScripts;

    [Header("Perks UI")]
    public GameObject perkPanel;
    public GameObject perkFrame;
    
    [Header("Player Perks")]
    public Perks[] perkList;
    public GameObject[] perkGameObjects;
    public GameObject perkParent;
    private List<Perks> selectedPerks = new List<Perks>();
    private const int maxPerkLevel = 5;
    public Dictionary<string, int> playerPerks = new Dictionary<string, int>();

    void Start()
    {
        BindButtons();
    }

    void Update()
    {
        // when I press J, cal the show all perk buttons
        if (Input.GetKeyDown(KeyCode.J))
        {
            ShowAllPerkButtons();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            PrintPerks();
        }

        // if I press Q print all the playerPerks
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PrintPerks();
        }
    }

    void BindSkills()
    {
        PickRandomPerks();
        // for loop selecting the perks
        for (int i = 0; i < perkButtons.Length; i++)
        {
            perkButtons[i].transform.GetChild(9).GetComponent<TextMeshProUGUI>().text = selectedPerks[i].perkDisplayName;
            if (playerPerks.ContainsKey(selectedPerks[i].perkName))
            {
                perkButtons[i].transform.GetChild(10).GetComponent<TextMeshProUGUI>().text = selectedPerks[i].perkDescriptions[playerPerks[selectedPerks[i].perkName]];
                perkButtons[i].transform.GetChild(11).GetComponent<TextMeshProUGUI>().text = "";
                float nextLevel = playerPerks[selectedPerks[i].perkName] + 1;
                perkButtons[i].transform.GetChild(12).GetComponent<TextMeshProUGUI>().text = "Lvl. " + nextLevel; 
            } else {
                perkButtons[i].transform.GetChild(10).GetComponent<TextMeshProUGUI>().text = selectedPerks[i].perkDescriptions[0];
                perkButtons[i].transform.GetChild(11).GetComponent<TextMeshProUGUI>().text = "<color=#FFB4A0>Cooldown: </color><color=#FFB023><b>" + selectedPerks[i].perkCooldown + "</b></color> <color=#FFFFFF>seconds</color>";
                perkButtons[i].transform.GetChild(12).GetComponent<TextMeshProUGUI>().text = "Lvl. 1";
            }
            perkButtons[i].transform.GetChild(6).GetComponent<Image>().sprite = Resources.Load<Sprite>("Perk Icons/" + selectedPerks[i].perkDisplayName) as Sprite;
            perkButtons[i].name = selectedPerks[i].perkName;
        }
    }

    void BindButtons()
    {
        perkAnimators = new Animator[perkButtons.Length];
        perkButtonScripts = new Button[perkButtons.Length];

        for (int i = 0; i < perkButtons.Length; i++)
        {
            perkAnimators[i] = perkButtons[i].GetComponent<Animator>();
            perkButtonScripts[i] = perkButtons[i].GetComponent<Button>();

            int index = i; // Capture a local copy of 'i'
            perkButtonScripts[i].onClick.AddListener(() => 
            {
                for (int j = 0; j < perkButtons.Length; j++)
                {
                    if (j == index)
                    {
                        perkAnimators[j].SetBool("isSelected", true);
                        perkMenuAnimator.SetBool("isSelected", true);
                        Debug.Log("Perk " + j + " is selected");
                        AddPerkToPlayer(selectedPerks[j]);
                    }
                    else
                    {
                        perkAnimators[j].SetBool("isNotSelected", true);
                    }
                }
            });
        }
    }

    public void HideAllPerkButtons()
    {
        timeManager.ResumeGame();
        for (int i = 0; i < perkButtons.Length; i++)
        {
            perkButtons[i].SetActive(false);
        }
        perkMenu.SetActive(false);
    }

    public void ShowAllPerkButtons()
    {
        timeManager.PauseGame();
        BindSkills();
        perkMenu.SetActive(true);
        for (int i = 0; i < perkButtons.Length; i++)
        {
            perkButtons[i].SetActive(true);
        }
    }

    private void PickRandomPerks()
    {
        selectedPerks.Clear();
        // Create a list of available perks to select from
        List<Perks> availablePerks = new List<Perks>(perkList);

        // Remove perks that are already at max level
        foreach (var perk in playerPerks)
        {
            Perks foundPerk = availablePerks.Find(p => p.perkName == perk.Key);
            if (foundPerk != null && perk.Value >= maxPerkLevel)
            {
                availablePerks.Remove(foundPerk);
            }
        }

        // Loop until you have selected 3 unique perks
        while (selectedPerks.Count < 3)
        {
            // If no available perks remain, break out of the loop
            if (availablePerks.Count == 0)
            {
                break;
            }

            // Filter out perks that are already at max level
            List<Perks> filteredPerks = availablePerks.FindAll(p => !playerPerks.ContainsKey(p.perkName) || playerPerks[p.perkName] < maxPerkLevel);

            // If no perks remain after filtering, break out of the loop
            if (filteredPerks.Count == 0)
            {
                break;
            }

            // Pick a random perk from the filtered list
            Perks randomPerk = filteredPerks[UnityEngine.Random.Range(0, filteredPerks.Count)];
            selectedPerks.Add(randomPerk);
            availablePerks.Remove(randomPerk); // Remove the selected perk from the available list
        }
    }

    private string FindImage(String perkName)
    {
        string path = "Assets/GUI/Skill Icons/Skills/" + perkName + ".png";

        if (File.Exists(path))
        {
            return path;
        }
        else
        {
            Debug.LogWarning("Image file " + perkName + " does not exist");
        }

        return null;
    }

    public void AddPerkToPlayer(Perks addedPerk)
    {
        if (playerPerks.ContainsKey(addedPerk.perkName))
        {
            if (playerPerks[addedPerk.perkName] < maxPerkLevel)
            {
                playerPerks[addedPerk.perkName]++;
                Debug.Log(addedPerk.perkName + " upgraded to level " + playerPerks[addedPerk.perkName]);
                foreach (Transform child in perkPanel.transform)
                {
                    if (child.name == addedPerk.perkName)
                    {
                        GameObject levelObject = child.Find("Level").gameObject;
                        TextMeshProUGUI levelText = levelObject.GetComponent<TextMeshProUGUI>();
                        levelText.text = "Lvl. " + playerPerks[addedPerk.perkName];
                    }
                }
            }
            else
            {
                Debug.Log(addedPerk.perkName + " is already at max level");
            }
        }
        else
        {
            playerPerks.Add(addedPerk.perkName, 1);
            GameObject newPerkFrame = Instantiate(perkFrame, perkPanel.transform);
            GameObject levelObject = newPerkFrame.transform.Find("Level").gameObject;
            TextMeshProUGUI levelText = levelObject.GetComponent<TextMeshProUGUI>();
            Image perkImage = newPerkFrame.transform.Find("Icon").GetComponent<Image>();

            for (int i = 0; i < perkGameObjects.Length; i++)
            {
                if (perkGameObjects[i].name == addedPerk.perkName)
                {
                    GameObject newPerk = Instantiate(perkGameObjects[i], perkParent.transform);
                    newPerk.name = addedPerk.perkName;
                }
                else
                {
                    Debug.LogWarning("Perk not found");
                }
            }

            newPerkFrame.name = addedPerk.perkName;
            // perkImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(FindImage(addedPerk.perkName));
            perkImage.sprite = Resources.Load<Sprite>("Perk Icons/" + addedPerk.perkDisplayName);
            levelText.text = "Lvl. 1";

            Debug.Log(addedPerk.perkName + " added at level 1");
        }
    }

    private void PrintPerks()
    {
        foreach (KeyValuePair<string, int> perk in playerPerks)
        {
            Debug.Log(perk.Key + " at level " + perk.Value);
        }
    }
}
