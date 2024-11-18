using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    private Rigidbody2D rb;

    public float strength = 0, delay = 0.15f;
    public PlayerStats playerStats;
    private GameObject player;
    private float rigidBodyMassDefault;

    private void Start()
    {
        strength = .3f;
        rb = GetComponent<Rigidbody2D>();
        // find with tag
        player = GameObject.FindWithTag("Player");

        // get the rigid body mass
        rigidBodyMassDefault = rb.mass;
    }

    public void AddForce()
    {
        StopAllCoroutines();
        rb.mass = 10;
        Vector2 direction = (transform.position - player.transform.position).normalized;
        rb.AddForce(direction * strength, ForceMode2D.Impulse);
        StartCoroutine(Reset());
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector2.zero;
        rb.mass = rigidBodyMassDefault;
    }
}
