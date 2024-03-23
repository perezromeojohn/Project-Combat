using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkManager : MonoBehaviour
{
    public GameObject[] perkButtons;
    public GameObject perkMenu;
    public Animator perkMenuAnimator;
    private Animator[] perkAnimators;
    private Button[] perkButtonScripts;

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
        for (int i = 0; i < perkButtons.Length; i++)
        {
            perkButtons[i].SetActive(false);
        }
        perkMenu.SetActive(false);
    }

    public void ShowAllPerkButtons()
    {
        perkMenu.SetActive(true);
        for (int i = 0; i < perkButtons.Length; i++)
        {
            perkButtons[i].SetActive(true);
        }
    }
}
