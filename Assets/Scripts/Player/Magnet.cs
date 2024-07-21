using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public PlayerStats playerStats;
    
    void Update()
    {
        // based on the magnet range, check for any drops within the range
        // tween it
        if (playerStats.magnetRange > 0)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, playerStats.magnetRange);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Drops"))
                {
                    if (collider.GetComponent<Drops>().canMagnet)
                    {
                        collider.gameObject.transform.position = Vector2.MoveTowards(collider.gameObject.transform.position, transform.position, 2f * Time.deltaTime);
                    }
                }
            }
        }
    }

    public void DisableMagnet()
    {
        enabled = false;
    }
}
