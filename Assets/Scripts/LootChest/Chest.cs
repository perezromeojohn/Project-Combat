using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private StatUpgrade[] stats;
    [SerializeField] private StatUpgrade chosenStat;
    [SerializeField] private PlayerStats playerStats;

    private BoxCollider2D boxCollider2D;
    public MMF_Player feedbacks;
    public bool isCollected = false;

    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        chosenStat = stats[Random.Range(0, stats.Length)];
    }

    void Update()
    {

    }

    void OpenChest()
    {

    }

    void CheckIfPerkIsMaxed()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;
            feedbacks.PlayFeedbacks();
        }
    }
}
