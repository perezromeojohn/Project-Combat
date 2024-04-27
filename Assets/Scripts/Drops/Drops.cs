using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using Unity.Mathematics;

public class Drops : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public GameObject icon;
    public GameObject collected;
    public enum ResourceType { XP, Health, PowerUp, Magnet, Coins, Gems, Immune }
    public ResourceType resourceToHandle;
    public float xpValue;

    public bool canMagnet = false;
    
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        StartCoroutine(EnableMagnet());

        // set scale of the icon to 0 and leantween them to 1 by .3 seconds
        icon.transform.localScale = new Vector3(0, 0, 0);
        if (resourceToHandle == ResourceType.XP)
        {
            LeanTween.scale(icon, new Vector3(0.7f, 0.7f, 0.7f), 0.3f);
        }
        else
        {
            LeanTween.scale(icon, new Vector3(1, 1, 1), 0.3f);
        }
    }

    IEnumerator EnableMagnet()
    {
        yield return new WaitForSeconds(1);
        canMagnet = true;
    }

    private void CollectXP()
    {
        GameObject levelManager = GameObject.Find("GameManager");
        LevelManager levelManagerScript = levelManager.GetComponent<LevelManager>();
        levelManagerScript.IncrementExperience((int)xpValue);
    }

    private void CollectHeal(GameObject player)
    {
        player.GetComponent<PlayerHealth>().HealDamage(1);
    }

    private void CollectCoin(GameObject player)
    {
        float coinValue = UnityEngine.Random.Range(15, 25);
        player.GetComponent<PlayerResourceData>().IncrementCoins(coinValue);
    }

    private void CollectGem(GameObject player)
    {
        float gemValue = 1f;
        player.GetComponent<PlayerResourceData>().IncrementGems(gemValue);
    }

    private void CollectPowerUp(GameObject player)
    {
        // Debug.Log("PowerUp");
    }  

    private void CollectImmune(GameObject player)
    {
        // Debug.Log("Immune");
    }

    private void CollectMagnet(GameObject player)
    {
        // Debug.Log("Magnet");
    }

    private void Operation(GameObject player)
    {
        switch (resourceToHandle)
        {
            case ResourceType.XP:
                CollectXP();
                break;
            case ResourceType.Health:
                CollectHeal(player);
                break;
            case ResourceType.PowerUp:
                CollectPowerUp(player);
                break;
            case ResourceType.Magnet:
                CollectMagnet(player);
                break;
            case ResourceType.Coins:
                CollectCoin(player);
                break;
            case ResourceType.Gems:
                CollectGem(player);
                break;
            case ResourceType.Immune:
                CollectImmune(player);
                break;
        }
    }

    // on collide debug the hit
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StopCoroutine(EnableMagnet());
            Operation(other.gameObject);
            Destroy(gameObject);
            // GameObject collectedAnim = Instantiate(collected, transform.position, Quaternion.identity);
            // GameObject debris = GameObject.Find("Debris");
            // collectedAnim.transform.SetParent(debris.transform);
        }
    }
}