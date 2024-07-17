using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject heartPrefab;
    List<HealthHeart> hearts = new List<HealthHeart>();

    private float maxHealth;
    private float health;

    void Start()
    {
        ClearHearts();
    }

    public float MaxHealth
    {
        get { return maxHealth; }
        set
        {
            maxHealth = value;
            DrawHearts(health, maxHealth); // Update the health bar when maxHealth changes
        }
    }

    public void DrawHearts(float health, float maxHealth)
    {
        this.health = health;
        this.maxHealth = maxHealth;

        if (hearts.Count == 0)
        {
            CreateEmptyHearts();
        }

        UpdateHeartStatus();
    }

    public void CreateEmptyHearts()
    {
        float maxHealthRemainder = maxHealth % 2;
        int heartsToMake = (int)(maxHealth / 2 + maxHealthRemainder);

        for (int i = 0; i < heartsToMake; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab);
            newHeart.transform.SetParent(transform);

            HealthHeart heartComponent = newHeart.GetComponent<HealthHeart>();
            heartComponent.SetHeartImage(HeartState.Empty);
            hearts.Add(heartComponent);

            StartCoroutine(CreateHeartWithDelay(i));
        }
    }

    IEnumerator CreateHeartWithDelay(int index)
    {
        yield return new WaitForSeconds(0.1f * index);
        LeanTween.scale(hearts[index].gameObject, new Vector3(1, 1, 1), 0.3f).setEase(LeanTweenType.easeOutQuart);
    }

    public void UpdateHeartStatus()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            int heartStatusRemainder = (int)Mathf.Clamp(health - (i * 2), 0, 2);
            hearts[i].SetHeartImage((HeartState)heartStatusRemainder);
        }
    }

    public void ClearHearts()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HealthHeart>();
    }

    public void UpdateMaxHealth(float addedPoints)
    {
        maxHealth += addedPoints;
        health += addedPoints;
        ClearHearts();
        DrawHearts(health, maxHealth);
    }
}
