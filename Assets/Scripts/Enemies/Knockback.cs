using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    private Rigidbody2D rb;

    public float strength = 0, delay = 0.15f;
    public PlayerStats playerStats;
    private GameObject player;

    private void Start()
    {
        strength = playerStats.knockbackStrength;
        rb = GetComponent<Rigidbody2D>();
        // find with tag
        player = GameObject.FindWithTag("Player");
    }

    public void AddForce()
    {
        StopAllCoroutines();
        Vector2 direction = (transform.position - player.transform.position).normalized;
        rb.AddForce(direction * strength, ForceMode2D.Impulse);
        StartCoroutine(Reset());
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector2.zero;
    }
}
