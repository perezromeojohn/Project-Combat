using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public GameObject[] hotBarFrame;
    
    [Header("Player Perks")]
    public Perks[] perkList;
    public GameObject[] perkGameObjects;

    [Header("Player Stat Upgrades")]
    public StatUpgrade[] statUpgrades;
    public GameObject perkParent;
    private List<object> selectedPerks = new List<object>();
    private const int maxPerkLevel = 5;
    private const int maxPerkToCollect = 8;
    private const int maxStatToCollect = 10;
    public float currentPerkCount = 0;
    public float currentStatCount = 0;
    public Dictionary<string, int> playerPerks = new Dictionary<string, int>();

    [Header("Player Stats")]
    public PlayerStats inGamePlayerStats;

    [Header("External Classes for Stats")]
    public PlayerHealth playerHealth;

    void Start()
    {
        BindButtons();


        // set up hotbar
        for (int i = 0; i < hotBarFrame.Length; i++)
        {
            Transform[] children = hotBarFrame[i].GetComponentsInChildren<Transform>();
            foreach (Transform child in children)
            {
                if (child.name == "Icon" || child.name == "Cooldown" || child.name == "Level")
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            ShowAllPerkButtons();
        }

        if (Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.Q))
        {
            PrintPerks();
        }
    }

    void BindSkills()
    {
        PickRandomUpgrades();
        for (int i = 0; i < perkButtons.Length; i++)
        {
            if (selectedPerks[i] is Perks perk)
            {
                BindPerk(perk, i);
            }
            else if (selectedPerks[i] is StatUpgrade statUpgrade)
            {
                BindStat(statUpgrade, i);
            }
        }
    }

    void BindPerk(Perks perk, int index)
    {
        perkButtons[index].transform.GetChild(9).GetComponent<TextMeshProUGUI>().text = perk.perkDisplayName;
        if (playerPerks.ContainsKey(perk.perkName))
        {
            perkButtons[index].transform.GetChild(10).GetComponent<TextMeshProUGUI>().text = perk.perkDescriptions[playerPerks[perk.perkName]];
            perkButtons[index].transform.GetChild(11).GetComponent<TextMeshProUGUI>().text = "";
            float nextLevel = playerPerks[perk.perkName] + 1;
            perkButtons[index].transform.GetChild(12).GetComponent<TextMeshProUGUI>().text = "Lvl. " + nextLevel; 
        } else {
            perkButtons[index].transform.GetChild(10).GetComponent<TextMeshProUGUI>().text = perk.perkDescriptions[0];
            perkButtons[index].transform.GetChild(11).GetComponent<TextMeshProUGUI>().text = "<color=#FFB4A0>Cooldown: </color><color=#FFB023><b>" + perk.perkCooldown + "</b></color> <color=#FFFFFF>seconds</color>";
            perkButtons[index].transform.GetChild(12).GetComponent<TextMeshProUGUI>().text = "Lvl. 1";
        }
        perkButtons[index].transform.GetChild(6).GetComponent<Image>().sprite = Resources.Load<Sprite>("Perk Icons/" + perk.perkDisplayName) as Sprite;
        perkButtons[index].name = perk.perkName;
    }

    void BindStat(StatUpgrade statUpgrade, int index)
    {
        perkButtons[index].transform.GetChild(9).GetComponent<TextMeshProUGUI>().text = statUpgrade.statDisplayName;
        if (playerPerks.ContainsKey(statUpgrade.statName))
        {
            int currentLevel = playerPerks[statUpgrade.statName];
            perkButtons[index].transform.GetChild(10).GetComponent<TextMeshProUGUI>().text = statUpgrade.statDescriptions[currentLevel];
            perkButtons[index].transform.GetChild(11).GetComponent<TextMeshProUGUI>().text = "";
            float nextLevel = currentLevel + 1;
            perkButtons[index].transform.GetChild(12).GetComponent<TextMeshProUGUI>().text = "Lvl. " + nextLevel;
        }
        else
        {
            perkButtons[index].transform.GetChild(10).GetComponent<TextMeshProUGUI>().text = statUpgrade.statDescriptions[0];
            perkButtons[index].transform.GetChild(11).GetComponent<TextMeshProUGUI>().text = "<color=#FFB4A0>Increase: </color><color=#FFB023><b>" + statUpgrade.baseIncreaseAmount + "</b></color>";
            perkButtons[index].transform.GetChild(12).GetComponent<TextMeshProUGUI>().text = "Lvl. 1";
        }
        perkButtons[index].transform.GetChild(6).GetComponent<Image>().sprite = Resources.Load<Sprite>("Stat Icons/" + statUpgrade.statDisplayName) as Sprite;
        perkButtons[index].name = statUpgrade.statName;
    }

    void BindButtons()
    {
        perkAnimators = new Animator[perkButtons.Length];
        perkButtonScripts = new Button[perkButtons.Length];

        for (int i = 0; i < perkButtons.Length; i++)
        {
            perkAnimators[i] = perkButtons[i].GetComponent<Animator>();
            perkButtonScripts[i] = perkButtons[i].GetComponent<Button>();

            int index = i;
            perkButtonScripts[i].onClick.AddListener(() => 
            {
                for (int j = 0; j < perkButtons.Length; j++)
                {
                    if (j == index)
                    {
                        perkAnimators[j].SetBool("isSelected", true);
                        perkMenuAnimator.SetBool("isSelected", true);
                        // Debug.Log("Perk " + j + " is selected");
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

    private void PickRandomUpgrades()
    {
        selectedPerks.Clear();
        List<object> availableUpgrades = new List<object>();
        
        availableUpgrades.AddRange(perkList.Where(p => !playerPerks.ContainsKey(p.perkName) || playerPerks[p.perkName] < maxPerkLevel));
        availableUpgrades.AddRange(statUpgrades.Where(s => !playerPerks.ContainsKey(s.statName) || playerPerks[s.statName] < maxPerkLevel));

        while (selectedPerks.Count < 3 && availableUpgrades.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableUpgrades.Count);
            object selectedUpgrade = availableUpgrades[randomIndex];
            selectedPerks.Add(selectedUpgrade);
            availableUpgrades.RemoveAt(randomIndex);
        }
    }

    public void AddPerkToPlayer(object addedUpgrade)
    {
        if (addedUpgrade is Perks perk)
        {
            AddPerkUpgrade(perk);
        }
        else if (addedUpgrade is StatUpgrade statUpgrade)
        {
            AddStatUpgrade(statUpgrade);
        }
    }

    private void AddPerkUpgrade(Perks perk)
    {
        if (playerPerks.ContainsKey(perk.perkName))
        {
            if (playerPerks[perk.perkName] < maxPerkLevel)
            {
                playerPerks[perk.perkName]++;
                // Debug.Log(perk.perkName + " upgraded to level " + playerPerks[perk.perkName]);
                UpdatePerkUI(perk);
                UpdatePerkToHotBar(perk);
            }
            else
            {
                // Debug.Log(perk.perkName + " is already at max level");
            }
        }
        else
        {
            playerPerks.Add(perk.perkName, 1);
            CreateNewPerkUI(perk);
            AddPerkToHotBar(perk);
            InstantiatePerkGameObject(perk);
            currentPerkCount++;
            // Debug.Log(perk.perkName + " added at level 1");
        }
    }

    private void AddStatUpgrade(StatUpgrade statUpgrade)
    {
        if (playerPerks.ContainsKey(statUpgrade.statName))
        {
            if (playerPerks[statUpgrade.statName] < maxPerkLevel)
            {
                playerPerks[statUpgrade.statName]++;
                // Debug.Log(statUpgrade.statName + " upgraded to level " + playerPerks[statUpgrade.statName]);
                UpdateStatUI(statUpgrade);
            }
            else
            {
                // Debug.Log(statUpgrade.statName + " is already at max level");
            }
        }
        else
        {
            playerPerks.Add(statUpgrade.statName, 1);
            CreateNewStatUI(statUpgrade);
            currentStatCount++;
            // Debug.Log(statUpgrade.statName + " added at level 1");
        }

        ApplyStatUpgrade(statUpgrade);
    }

    private void UpdatePerkUI(Perks perk)
    {
        // Update existing perk UI
        foreach (Transform child in perkPanel.transform)
        {
            if (child.name == perk.perkName)
            {
                GameObject levelObject = child.Find("Level").gameObject;
                TextMeshProUGUI levelText = levelObject.GetComponent<TextMeshProUGUI>();
                levelText.text = "Lvl. " + playerPerks[perk.perkName];
                break;
            }
        }
    }

    private void UpdateStatUI(StatUpgrade statUpgrade)
    {
        // Update existing stat UI
        foreach (Transform child in perkPanel.transform)
        {
            if (child.name == statUpgrade.statName)
            {
                GameObject levelObject = child.Find("Level").gameObject;
                TextMeshProUGUI levelText = levelObject.GetComponent<TextMeshProUGUI>();
                levelText.text = "Lvl. " + playerPerks[statUpgrade.statName];
                break;
            }
        }
    }

    private void CreateNewPerkUI(Perks perk)
    {
        GameObject newPerkFrame = Instantiate(perkFrame, perkPanel.transform);
        GameObject levelObject = newPerkFrame.transform.Find("Level").gameObject;
        TextMeshProUGUI levelText = levelObject.GetComponent<TextMeshProUGUI>();
        Image perkImage = newPerkFrame.transform.Find("Icon").GetComponent<Image>();

        newPerkFrame.name = perk.perkName;
        perkImage.sprite = Resources.Load<Sprite>("Perk Icons/" + perk.perkDisplayName);
        levelText.text = "Lvl. 1";
    }

    private void CreateNewStatUI(StatUpgrade statUpgrade)
    {
        GameObject newStatFrame = Instantiate(perkFrame, perkPanel.transform);
        GameObject levelObject = newStatFrame.transform.Find("Level").gameObject;
        TextMeshProUGUI levelText = levelObject.GetComponent<TextMeshProUGUI>();
        Image statImage = newStatFrame.transform.Find("Icon").GetComponent<Image>();

        newStatFrame.name = statUpgrade.statName;
        statImage.sprite = Resources.Load<Sprite>("Stat Icons/" + statUpgrade.statDisplayName);
        levelText.text = "Lvl. 1";
    }

    private void InstantiatePerkGameObject(Perks perk)
    {
        for (int i = 0; i < perkGameObjects.Length; i++)
        {
            if (perkGameObjects[i].name == perk.perkName)
            {
                GameObject newPerk = Instantiate(perkGameObjects[i], perkParent.transform);
                newPerk.name = perk.perkName;
                return;
            }
        }
        Debug.LogWarning("Perk not found");
    }

    private void ApplyStatUpgrade(StatUpgrade statUpgrade)
    {
        switch (statUpgrade.statName)
        {
            case "Health":
                inGamePlayerStats.maxHealth += statUpgrade.baseIncreaseAmount;
                playerHealth.UpdateMaxHealth(statUpgrade.baseIncreaseAmount);
                break;
            case "Damage":
                inGamePlayerStats.damage += statUpgrade.baseIncreaseAmount;
                break;
            case "Crit Chance":
                inGamePlayerStats.critChance += statUpgrade.baseIncreaseAmount;
                break;
            case "Crit Damage":
                inGamePlayerStats.critDamage += statUpgrade.baseIncreaseAmount;
                break;
            case "Magnet Range":
                inGamePlayerStats.magnetRange += statUpgrade.baseIncreaseAmount;
                break;
            case "Movement Speed":
                inGamePlayerStats.movementSpeed += statUpgrade.baseIncreaseAmount;
                break;
            // Add other stats as needed
        }
    }

    private void AddPerkToHotBar(Perks perk)
    {
        GameObject hotBarFrame = this.hotBarFrame[(int)currentPerkCount];
        hotBarFrame.name = perk.perkName;

        GameObject levelObject = hotBarFrame.transform.Find("Level").gameObject;
        TextMeshProUGUI levelText = levelObject.GetComponent<TextMeshProUGUI>();
        levelText.text = "Lvl. 1";

        Image perkImage = hotBarFrame.transform.Find("Icon").GetComponent<Image>();
        perkImage.sprite = Resources.Load<Sprite>("Perk Icons/" + perk.perkDisplayName);

        // active
        hotBarFrame.transform.Find("Icon").gameObject.SetActive(true);
        // hotBarFrame.transform.Find("Cooldown").gameObject.SetActive(true);
        hotBarFrame.transform.Find("Level").gameObject.SetActive(true);
        hotBarFrame.transform.Find("Cooldown").gameObject.SetActive(true);
    }

    private void UpdatePerkToHotBar(Perks perk)
    {
        // find the perk in the hotbar
        for (int i = 0; i < hotBarFrame.Length; i++)
        {
            if (hotBarFrame[i].name == perk.perkName)
            {
                GameObject levelObject = hotBarFrame[i].transform.Find("Level").gameObject;
                TextMeshProUGUI levelText = levelObject.GetComponent<TextMeshProUGUI>();
                levelText.text = "Lvl. " + playerPerks[perk.perkName];
                break;
            }
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