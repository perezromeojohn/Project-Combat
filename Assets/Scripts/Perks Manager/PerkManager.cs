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
    public Perks[] perks;
    private List<Perks> selectedPerks = new List<Perks>();

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
    }

    void BindSkills()
    {
        PickRandomPerks();
        // for loop selecting the perks
        for (int i = 0; i < perkButtons.Length; i++)
        {
            perkButtons[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = selectedPerks[i].perkName;
            perkButtons[i].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = selectedPerks[i].perkDescription;
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
            Perks randomPerk = perks[UnityEngine.Random.Range(0, perks.Length)];
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
}
