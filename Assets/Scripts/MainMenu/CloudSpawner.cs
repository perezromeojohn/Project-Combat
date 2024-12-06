using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject cloudPrefab;
    public float spawnInterval = 3.0f;
    private SpriteRenderer spriteRenderer;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = spawnInterval;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SpawnCloud();
            timer = spawnInterval;
        }
    }

    void SpawnCloud()
    {
        float x = Random.Range(spriteRenderer.bounds.min.x, spriteRenderer.bounds.max.x);
        float y = Random.Range(spriteRenderer.bounds.min.y, spriteRenderer.bounds.max.y);
        Vector3 spawnPosition = new Vector3(x, y, 0);
        Instantiate(cloudPrefab, spawnPosition, Quaternion.identity);
    }
}
