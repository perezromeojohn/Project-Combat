using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    public float speed = .1f;
    public float positionToDestroy = -3;
    private SpriteRenderer sp;
    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        if (Random.Range(0, 2) == 1)
        {
            sp.flipX = true;
        }
    }

    // make the clouds move to the left side. after 10 seconds destroy the clouds
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
        if (transform.position.x < positionToDestroy)
        {
            Destroy(gameObject);
        }
    }
}
