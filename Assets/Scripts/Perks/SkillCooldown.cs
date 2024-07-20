using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SkillCooldown : MonoBehaviour
{
    private float cooldownDuration = 0f;
    public float cooldownTimer = 0f;
    public bool isOnCooldown = false;

    public Component aboveComponent; // Store the reference to the component above
    private Image skillCooldownSprite;

    private void OnEnable()
    {
        GetComponentAbove();
    }

    public void StartCooldown(float duration)
    {
        cooldownDuration = duration;
        cooldownTimer = 0f;
        isOnCooldown = true;
    }

    void Start()
    {
        // find the GameObject named HotBarPanel
        // get its children. now base on the component name, print the gameobject that matches the name inside the HotBarPanel
        GameObject hotBarPanel = GameObject.Find("HotBarPanel");
        Transform[] children = hotBarPanel.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child.gameObject.name == aboveComponent.gameObject.name)
            {
                skillCooldownSprite = child.transform.GetChild(1).GetComponent<Image>();
            }
        }
    }

    private void Update()
    {
        if (isOnCooldown)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= cooldownDuration)
            {
                Debug.Log("Cooldown finished!");
                cooldownTimer = 0f;
                isOnCooldown = false;
            }

            // the image is a radial 360 sprite
            // the fill amount is the percentage of the image that is filled
            skillCooldownSprite.fillAmount = cooldownTimer / cooldownDuration;
        }
    }

    private void GetComponentAbove()
    {
        Component[] components = GetComponents<Component>();
        for (int i = 0; i < components.Length; i++)
        {
            if (components[i] == this && i > 1)
            {
                aboveComponent = components[i - 1];
                Debug.Log("Component above SkillCooldown: " + aboveComponent.GetType().Name);
                return;
            }
        }
        aboveComponent = null;
    }
}
