using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDistribution : MonoBehaviour
{
    public GameObject xpDrop;
    public GameObject[] drops;

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
        if (dropChance <= 30)
        {
            if (drops.Length == 0)
            {
                Debug.Log("No Drops Available!");
            }
            else
            {
                // randomize .1f to .1f using this gameobject's position
                var randomPos = new Vector3(pos.x + randomX, pos.y + randomY, pos.z);
                var cloneDrop = Instantiate(drops[Random.Range(0, drops.Length)], randomPos, Quaternion.identity, dropLayer.transform);
                cloneDrop.transform.SetParent(dropLayer.transform);
            }
        }
        var randomXDrp = Random.Range(-.1f, .1f);
        var randomYDrp = Random.Range(-.1f, .1f);
        var randomPosDrp = new Vector3(pos.x + randomXDrp, pos.y + randomYDrp, pos.z);
        var cloneXp = Instantiate(xpDrop, randomPosDrp, Quaternion.identity, dropLayer.transform);
        cloneXp.transform.SetParent(dropLayer.transform);
    }
}
