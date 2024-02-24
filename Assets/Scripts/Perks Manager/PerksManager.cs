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
    public Image levelUpWindowImage;
    public List<Perks> perksList;

    [Header("GUI Stuff")]
    public List<GameObject> buttons;
    public GameObject topBar;
    public GameObject bottomBar1;
    public GameObject bottomBar2;
    
    public void OpenLevelUpWindow()
    {
        UpdateButtons();
        levelUpWindow.SetActive(true);
        // also stop the game time
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

        StartGUIAnims();
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

    private IEnumerator PlayAnims()
    {
        // for loop the button, and enable the Button component
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().enabled = true;
        }
        // enable the level up window
        levelUpWindow.SetActive(true);
        // get the levelUpWindowImage and tween its rgba to 0, 0, 0, 135 using value
        LeanTween.value(levelUpWindowImage.gameObject, new Color(0, 0, 0, 0), new Color(0, 0, 0, .5f), 0.3f).setEaseOutQuad().setOnUpdate((Color val) =>
        {
            levelUpWindowImage.color = val;
        });
        RectTransform topBarRectTransform = topBar.GetComponent<RectTransform>();
        topBarRectTransform.anchoredPosition = new Vector2(0, 150);
        LeanTween.moveY(topBarRectTransform, 0, 0.5f).setEaseOutQuad();

        // set rec transform of bottomFrame1's Y to -315 and then tween it to 165
        RectTransform bottomBar1RectTransform = bottomBar1.GetComponent<RectTransform>();
        bottomBar1RectTransform.anchoredPosition = new Vector2(0, -300);
        LeanTween.moveY(bottomBar1RectTransform, 76.5f, 0.8f).setEaseOutQuad();

        // set rec transform of bottomFrame2's Y to -315 and then tween it to 165.5
        RectTransform bottomBar2RectTransform = bottomBar2.GetComponent<RectTransform>();
        bottomBar2RectTransform.anchoredPosition = new Vector2(0, -315);
        LeanTween.moveY(bottomBar2RectTransform, 165.5f, 0.5f).setEaseOutQuad();

        // get the buttons, get the image and set alpha to zero, also set the rect transform of the button's y to 0
        foreach (GameObject button in buttons)
        {
            RectTransform buttonRectTransform = button.GetComponent<RectTransform>();
            buttonRectTransform.anchoredPosition = new Vector2(buttonRectTransform.anchoredPosition.x, 0);
            // also set the rectransform scale to 0
            button.transform.localScale = new Vector3(0, 0, 0);
            // get all Image components in the buttons' children, set their alpha to zero. get all text mesh pro components in the buttons, set their vertexcolor alpha to 0
            Image[] images = button.GetComponentsInChildren<Image>();
            foreach (Image image in images)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
            }

            TextMeshProUGUI[] texts = button.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI text in texts)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
            }
        }

        // now tween the alpha to 1, set the rect transform of the button's y to -44 with a delay of 0.5 seconds for each button
        foreach (GameObject button in buttons)
        {
            yield return new WaitForSeconds(.2f);
            button.transform.localScale = new Vector3(.9f, .9f, 0);
            RectTransform buttonRectTransform = button.GetComponent<RectTransform>();
            LeanTween.moveY(buttonRectTransform, -44, 0.5f).setEaseOutQuad();
            // get all Image components in the buttons' children, set their alpha to zero. get all text mesh pro components in the buttons, set their vertexcolor alpha to 0
            Image[] images = button.GetComponentsInChildren<Image>();
            foreach (Image image in images)
            {
                LeanTween.alpha(image.rectTransform, 1, 0.5f).setEaseOutQuad();
            }

            TextMeshProUGUI[] texts = button.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI text in texts)
            {
                LeanTween.value(text.gameObject, 0, 1, 0.5f).setEaseOutQuad().setOnUpdate((float val) =>
                {
                    text.color = new Color(text.color.r, text.color.g, text.color.b, val);
                });
            }
        }
    }

    private IEnumerator EndAnims()
    {
        // for loop the button, and disable the Button component
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().enabled = false;
        }
        // set rec transform of topFrame's Y to 0 and then tween it to 150
        RectTransform topBarRectTransform = topBar.GetComponent<RectTransform>();
        topBarRectTransform.anchoredPosition = new Vector2(0, 0);
        LeanTween.moveY(topBarRectTransform, 150, 0.5f).setEaseOutQuad();

        // set rec transform of bottomFrame1's Y to 76.5 and then tween it to -315
        RectTransform bottomBar1RectTransform = bottomBar1.GetComponent<RectTransform>();
        bottomBar1RectTransform.anchoredPosition = new Vector2(0, 76.5f);
        LeanTween.moveY(bottomBar1RectTransform, -300, 0.8f).setEaseOutQuad();

        // set rec transform of bottomFrame2's Y to 165.5 and then tween it to -315
        RectTransform bottomBar2RectTransform = bottomBar2.GetComponent<RectTransform>();
        bottomBar2RectTransform.anchoredPosition = new Vector2(0, 165.5f);
        LeanTween.moveY(bottomBar2RectTransform, -315, 0.5f).setEaseOutQuad();

        // now tween the alpha to 1, set the rect transform of the button's y to -44 with a delay of 0.5 seconds for each button
        foreach (GameObject button in buttons)
        {
            yield return new WaitForSeconds(.1f);
            RectTransform buttonRectTransform = button.GetComponent<RectTransform>();
            LeanTween.moveY(buttonRectTransform, 44, 0.5f).setEaseOutQuad();
            // get all Image components in the buttons' children, set their alpha to zero. get all text mesh pro components in the buttons, set their vertexcolor alpha to 0
            Image[] images = button.GetComponentsInChildren<Image>();
            foreach (Image image in images)
            {
                LeanTween.alpha(image.rectTransform, 0, 0.5f).setEaseOutQuad();
            }

            TextMeshProUGUI[] texts = button.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI text in texts)
            {
                LeanTween.value(text.gameObject, 1, 0, 0.5f).setEaseOutQuad().setOnUpdate((float val) =>
                {
                    text.color = new Color(text.color.r, text.color.g, text.color.b, val);
                });
            }
        }

        // get the levelUpWindow image component and tween its alpha to 0
       LeanTween.value(levelUpWindowImage.gameObject, new Color(0, 0, 0, .5f), new Color(0, 0, 0, 0), 0.3f).setEaseOutQuad().setOnUpdate((Color val) =>
        {
            levelUpWindowImage.color = val;
        });

        yield return new WaitForSeconds(.3f);
        // disable the level up window
        levelUpWindow.SetActive(false);
    }

    public void StartGUIAnims()
    {
        StartCoroutine(PlayAnims());
    }

    public void EndGUIAnims()
    {
        StartCoroutine(EndAnims());
    }


    [CustomEditor(typeof(PerksManager))]
    public class PerksManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("GUI Animations"))
            {
                PerksManager perksManager = (PerksManager)target;
                perksManager.StartGUIAnims();
            }

            if (GUILayout.Button("End GUI Animations"))
            {
                PerksManager perksManager = (PerksManager)target;
                perksManager.EndGUIAnims();
            }
        }
    }
}
