using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;
    public float pierce;
    private Collider2D col;
    private List<Collider2D> hitEnemies = new List<Collider2D>();
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hitEnemies.Contains(other))
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                hitEnemies.Add(other);
                other.GetComponent<Behavior>().isAttacked = true;
                other.GetComponent<Behavior>().damageTaken = damage;
                pierce--;
                if (pierce <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
