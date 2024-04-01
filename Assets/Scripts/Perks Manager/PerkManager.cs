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
    
    [Header("Player Perks")]
    public Perks[] perkList;
    private List<Perks> selectedPerks = new List<Perks>();
    private const int maxPerkLevel = 5;
    public Dictionary<string, int> playerPerks = new Dictionary<string, int>();
    public GameObject[] perkFrames; // gui indicating the frame of the Perk
    private float perkFrameIndex = 0;

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
    }

    void BindSkills()
    {
        PickRandomPerks();
        // for loop selecting the perks
        for (int i = 0; i < perkButtons.Length; i++)
        {
            perkButtons[i].transform.GetChild(9).GetComponent<TextMeshProUGUI>().text = selectedPerks[i].perkName;
            perkButtons[i].transform.GetChild(10).GetComponent<TextMeshProUGUI>().text = selectedPerks[i].perkDescription;
            // for cooldown set format to color <color=#red>: Cooldown: color <color=#yellow>: perkCooldown seconds
            perkButtons[i].transform.GetChild(11).GetComponent<TextMeshProUGUI>().text = "<color=#FFB4A0>Cooldown: </color><color=#FFB023><b>" + selectedPerks[i].perkCooldown + "</b></color> <color=#FFFFFF>seconds</color>";
            perkButtons[i].transform.GetChild(6).GetComponent<Image>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>(FindImage(selectedPerks[i].perkName));
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
        for (int i = 0; i < 3; i++)
        {
            Perks randomPerk = perkList[UnityEngine.Random.Range(0, perkList.Length)];
            if (!selectedPerks.Contains(randomPerk))
            {
                selectedPerks.Add(randomPerk);
            }
            else
            {
                i--;
            }
        }
    }

    private string FindImage(String perkName)
    {
        // find the image of the perk image based on the name
        // directory: Assets/GUI/Skill Icons / Skills / perkname.png
        // return the image asset

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
            }
            else
            {
                Debug.Log(addedPerk.perkName + " is already at max level");
            }
        }
        else
        {
            playerPerks.Add(addedPerk.perkName, 1);
            Debug.Log(addedPerk.perkName + " added at level 1");
            perkFrames[Convert.ToInt32(perkFrameIndex)].GetComponent<Image>().enabled = true;
            perkFrames[Convert.ToInt32(perkFrameIndex)].GetComponent<Image>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>(FindImage(addedPerk.perkName));
            if (perkFrameIndex < 5)
            {
                perkFrameIndex++;
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
