using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggAnim : MonoBehaviour
{
    public GameObject egg;
    public Collider2D eggCollider;
    public float damage = 0;
    private List<Collider2D> hitEnemies = new List<Collider2D>();

    void End()
    {
        eggCollider.enabled = false;
    }

    void Destroy()
    {
        Destroy(egg);
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
            }
        }
    }
}
