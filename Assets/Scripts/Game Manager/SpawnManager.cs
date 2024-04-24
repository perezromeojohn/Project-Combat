using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

[System.Serializable]
public class SpawnRule
{
    public float spawnTime; // The time at which this rule applies
    public int batchSpawnAmount;
    public List<string> enemyTypes; // List of enemy types to spawn at this time
}

public class SpawnManager : MonoBehaviour
{
    public List<Transform> spawnPoints;
    public List<GameObject> enemyPrefabs;
    private GameObject player;
    public GameObject instantiatedEnemiesParent;
    public float disableRadius = 5f;

    public TimeManager timeManager;

    public List<SpawnRule> spawnRules; // List of spawn rules based on time

    public int maxUnitCap = 100;

    private float lastSpawnTime;
    private float spawnInterval = 5;
    private float previousSpawnTime = 0f; // Track previous spawn time

    void Awake()
    {
        spawnRules.Add(new SpawnRule { spawnTime = 0f, batchSpawnAmount = 20, enemyTypes = new List<string> { "Gobbie" } });
        // spawnRules.Add(new SpawnRule { spawnTime = 20, batchSpawnAmount = 15, enemyTypes = new List<string> { "Skellie" } });
        // spawnRules.Add(new SpawnRule { spawnTime = 30, batchSpawnAmount = 15, enemyTypes = new List<string> { "Skellie", "Gobbie" } });
        // spawnRules.Add(new SpawnRule { spawnTime = 40, batchSpawnAmount = 10, enemyTypes = new List<string> { "Grizzly", "Yellow Skellie" } });
        // spawnRules.Add(new SpawnRule { spawnTime = 50, batchSpawnAmount = 10, enemyTypes = new List<string> { "Bandit Gobbie", "Red Gobbie" } });
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lastSpawnTime = Time.time;
    }

    void Update()
    {
        if (Time.time - lastSpawnTime >= spawnInterval)
        {
            lastSpawnTime = Time.time;
            SpawnMobs();
        }

        // if I press K, return the current time
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Current time: " + timeManager.GetTimeElapsed()); // this returns the current time elapsed
        }
    }

    public void SpawnMobs()
    {
        if (instantiatedEnemiesParent.transform.childCount >= maxUnitCap)
        {
            Debug.Log("Unit cap reached. No more spawning.");
            return;
        }

        EnableNearbySpawnPoints();
        DisableNearbySpawnPoints();

        float elapsedTime = timeManager.GetTimeElapsed();
        Debug.Log("Elapsed Time: " + elapsedTime);

        foreach (SpawnRule rule in spawnRules)
        {
            if (elapsedTime >= rule.spawnTime && rule.spawnTime >= previousSpawnTime)
            {
                Debug.Log("Spawning enemies for time: " + rule.spawnTime);
                for (int i = 0; i < rule.batchSpawnAmount; i++)
                {
                    foreach (string enemyType in rule.enemyTypes)
                    {
                        GameObject prefab = GetEnemyPrefabByType(enemyType);
                        if (prefab != null)
                        {
                            Vector3 spawnPosition = GetRandomSpawnPosition(GetRandomSpawnPoint(GetActiveSpawnPoints()));
                            Instantiate(prefab, spawnPosition, Quaternion.identity, instantiatedEnemiesParent.transform);
                            Debug.Log("Spawned enemy: " + enemyType);
                        }
                    }
                }
                // Update previous spawn time
                previousSpawnTime = rule.spawnTime;
            }
        }
    }

    private GameObject GetEnemyPrefabByType(string mobType)
    {
        foreach (GameObject enemyPrefab in enemyPrefabs)
        {
            if (enemyPrefab.name == mobType)
            {
                return enemyPrefab;
            }
        }

        Debug.LogWarning("No enemy prefab found for mob type " + mobType);
        return null;
    }

    private List<Transform> GetActiveSpawnPoints()
    {
        List<Transform> activeSpawnPoints = new List<Transform>();
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint.gameObject.activeSelf)
            {
                activeSpawnPoints.Add(spawnPoint);
            }
        }
        return activeSpawnPoints;
    }

    private Transform GetRandomSpawnPoint(List<Transform> spawnPoints)
    {
        return spawnPoints[Random.Range(0, spawnPoints.Count)];
    }

    void DisableNearbySpawnPoints()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (Vector3.Distance(spawnPoint.position, player.transform.position) <= disableRadius)
            {
                spawnPoint.gameObject.SetActive(false);
            }
        }
    }

    void EnableNearbySpawnPoints()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            spawnPoint.gameObject.SetActive(true);
        }
    }

    Vector3 GetRandomSpawnPosition(Transform spawnPoint)
    {
        Vector3 spawnPosition = Vector3.zero;
        SpriteRenderer renderer = spawnPoint.GetComponent<SpriteRenderer>();

        if (renderer != null)
        {
            Bounds bounds = renderer.bounds;
            spawnPosition = new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), spawnPoint.position.z);
        }

        return spawnPosition;
    }

    GameObject GetRandomEnemyPrefab()
    {
        return enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
    }
}
