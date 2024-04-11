using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    public List<Transform> spawnPoints;
    public List<GameObject> enemyPrefabs;
    private GameObject player;
    public GameObject instantiatedEnemiesParent;
    public float disableRadius = 5f;
    public List<WaveData> waves;
    
    private float childCount;
    public int maxUnitCap = 100;

    private float lastSpawnTime;
    private float spawnInterval = 2f;

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

        int remainingSlots = maxUnitCap - instantiatedEnemiesParent.transform.childCount;
        int spawnCount = Mathf.Min(10, remainingSlots); // Spawn up to 10 mobs or remaining slots

        foreach (GameObject prefab in enemyPrefabs)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition(GetRandomSpawnPoint(GetActiveSpawnPoints()));
            Instantiate(prefab, spawnPosition, Quaternion.identity, instantiatedEnemiesParent.transform);
        }
    }

    private GameObject GetEnemyPrefabByType(string mobType)
    {
        foreach (GameObject enemyPrefab in enemyPrefabs)
        {
            if (enemyPrefab.name.Contains(mobType))
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

    void AddWaves()
    {
        waves = new List<WaveData>();
        waves.Add(new WaveData(1.0f, 1.0f, 1.0f, 30, new Dictionary<MobType, int>
        {
            { MobType.Gobbie, 80 },
        }));
        waves.Add(new WaveData(1.2f, 1.1f, 1.2f, 30, new Dictionary<MobType, int>
        {
            { MobType.Gobbie, 80 },
            { MobType.Blue_Skellie, 10 },
        }));    
        waves.Add(new WaveData(1.5f, 1.2f, 1.5f, 30, new Dictionary<MobType, int>
        {
            { MobType.Gobbie, 80 },
            { MobType.Pumpkin_Gobbie, 10 },
        }));
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
