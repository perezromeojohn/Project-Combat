using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotProjectile : MonoBehaviour
{
    public float damage;
    private List<Collider2D> hitEnemies = new List<Collider2D>();
    private Collider2D col;
    public SpriteRenderer carrotSprite;

    void Start()
    {
        col = GetComponent<Collider2D>();
        StartCoroutine(ClearHitListRoutine());
    }

    void Update()
    {
        carrotSprite.transform.Rotate(0, 0, 360 * Time.deltaTime);
    }

    IEnumerator ClearHitListRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            hitEnemies.Clear();
        }
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
