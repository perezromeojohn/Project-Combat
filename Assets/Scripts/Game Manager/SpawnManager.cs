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

    private Camera mainCamera;
    public float spawnOffset = 0.3f; // Minimum offset to ensure enemies spawn outside the camera view
    public float maxSpawnOffset = 1.0f; // Maximum offset for random spawn positions

    public GameObject playArea; // Reference to the scaled GameObject representing the play area
    private Bounds playAreaBounds;

    void Awake()
    {
        // Example spawn rules
        spawnRules.Add(new SpawnRule { spawnTime = 0f, batchSpawnAmount = 20, enemyTypes = new List<string> { "Gobbie", "Skellie" } });
        spawnRules.Add(new SpawnRule { spawnTime = 20, batchSpawnAmount = 15, enemyTypes = new List<string> { "Skellie" } });
        spawnRules.Add(new SpawnRule { spawnTime = 30, batchSpawnAmount = 15, enemyTypes = new List<string> { "Skellie", "Gobbie" } });
        spawnRules.Add(new SpawnRule { spawnTime = 40, batchSpawnAmount = 10, enemyTypes = new List<string> { "Grizzly", "Yellow Skellie" } });
        spawnRules.Add(new SpawnRule { spawnTime = 50, batchSpawnAmount = 10, enemyTypes = new List<string> { "Bandit Gobbie", "Red Gobbie" } });
        spawnRules.Add(new SpawnRule { spawnTime = 60, batchSpawnAmount = 10, enemyTypes = new List<string> { "Gobbie", "Skellie", "Red Gobbie", "Blue Skellie", "Grizzly" } });

        if (playArea != null)
        {
            playAreaBounds = playArea.GetComponent<Renderer>().bounds;
        }
        else
        {
            Debug.LogError("Play area GameObject not assigned to SpawnManager!");
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lastSpawnTime = Time.time;
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Time.time - lastSpawnTime >= spawnInterval)
        {
            lastSpawnTime = Time.time;
            SpawnMobs();
        }

        // If I press K, return the current time
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Current time: " + timeManager.GetTimeElapsed()); // This returns the current time elapsed
        }
    }

    public void SpawnMobs()
    {
        if (instantiatedEnemiesParent.transform.childCount >= maxUnitCap)
        {
            Debug.Log("Unit cap reached. No more spawning.");
            return;
        }

        float elapsedTime = timeManager.GetTimeElapsed();
        Debug.Log("Elapsed Time: " + elapsedTime);

        foreach (SpawnRule rule in spawnRules)
        {
            if (elapsedTime >= rule.spawnTime && rule.spawnTime >= previousSpawnTime)
            {
                Debug.Log("Spawning enemies for time: " + rule.spawnTime);
                int remainingBatchAmount = rule.batchSpawnAmount;

                for (int i = 0; i < rule.enemyTypes.Count && remainingBatchAmount > 0; i++)
                {
                    string enemyType = rule.enemyTypes[i];
                    GameObject prefab = GetEnemyPrefabByType(enemyType);

                    if (prefab != null)
                    {
                        int spawnCount = Mathf.Min(remainingBatchAmount, rule.batchSpawnAmount / rule.enemyTypes.Count);
                        
                        for (int j = 0; j < spawnCount; j++)
                        {
                            Vector3 spawnPosition = GetRandomSpawnPosition();
                            Instantiate(prefab, spawnPosition, Quaternion.identity, instantiatedEnemiesParent.transform);
                            Debug.Log("Spawned enemy: " + enemyType);
                        }

                        remainingBatchAmount -= spawnCount;
                    }
                }

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

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 spawnPosition;
        bool validPosition = false;

        do
        {
            // Get a random position within the play area bounds
            spawnPosition = new Vector3(
                Random.Range(playAreaBounds.min.x, playAreaBounds.max.x),
                Random.Range(playAreaBounds.min.y, playAreaBounds.max.y),
                0
            );

            // Check if the position is outside the camera view
            Vector3 viewportPoint = mainCamera.WorldToViewportPoint(spawnPosition);
            validPosition = viewportPoint.x < 0 || viewportPoint.x > 1 || 
                            viewportPoint.y < 0 || viewportPoint.y > 1;

        } while (!validPosition);

        return spawnPosition;
    }
}
