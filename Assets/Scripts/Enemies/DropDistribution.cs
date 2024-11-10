using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDistribution : MonoBehaviour
{
    public GameObject xpDrop;
    public GameObject[] drops;
    public float itemDropPercentage = 1;

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
        int dropChance = Random.Range(0, 500);
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

        var xpRandomX = Random.Range(-.1f, .1f);
        var xpRandomY = Random.Range(-.1f, .1f);
        var randomPosDrp = new Vector3(pos.x + xpRandomX, pos.y + xpRandomY, pos.z);
        var cloneXp = Instantiate(xpDrop, randomPosDrp, Quaternion.identity, dropLayer.transform);
        cloneXp.GetComponent<Drops>().xpValue = gameObject.GetComponent<Behavior>().enemy.experienceValue;
        cloneXp.transform.SetParent(dropLayer.transform);
    }
}
