using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDistribution : MonoBehaviour
{
    public GameObject xpDrop;
    public GameObject[] drops;
    public GameObject coin;
    public float itemDropPercentage = 20;
    public float coinDropPercentage = 80;

    private GameObject dropLayer;

    void Start()
    {
        dropLayer = GameObject.Find("Drops Layer");
    }

    public void DropItem()
    {
        var pos = transform.position;
        var randomX = Random.Range(-.1f, .1f);
        var randomY = Random.Range(-.1f, .1f);
        int dropChance = Random.Range(0, 100);
        if (dropChance <= itemDropPercentage)
        {
            if (drops.Length == 0)
            {
                Debug.Log("No Drops Available!");
            }
            else
            {
                var randomPos = new Vector3(pos.x + randomX, pos.y + randomY, pos.z);
                var cloneDrop = Instantiate(drops[Random.Range(0, drops.Length)], randomPos, Quaternion.identity, dropLayer.transform);
                cloneDrop.transform.SetParent(dropLayer.transform);
            }
        }
        if (dropChance <= coinDropPercentage)
        {
            DropCoin();
        }
        var randomPosDrp = new Vector3(pos.x + randomX, pos.y + randomY, pos.z);
        var cloneXp = Instantiate(xpDrop, randomPosDrp, Quaternion.identity, dropLayer.transform);
        cloneXp.GetComponent<Drops>().xpValue = gameObject.GetComponent<Behavior>().enemy.experienceValue;
        cloneXp.transform.SetParent(dropLayer.transform);
    }

    public void DropCoin()
    {
        var pos = transform.position;
        var randomX = Random.Range(-.1f, .1f);
        var randomY = Random.Range(-.1f, .1f);
        var randomPos = new Vector3(pos.x + randomX, pos.y + randomY, pos.z);
        var cloneCoin = Instantiate(coin, randomPos, Quaternion.identity, dropLayer.transform);
        cloneCoin.transform.SetParent(dropLayer.transform);
    }
}
