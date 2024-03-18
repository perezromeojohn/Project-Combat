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
    private float currentWaveIndex = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        AddWaves();
        SpawnEnemies(currentWaveIndex);
        Debug.Log("Total Waves: " + waves.Count);
    }

    void Update()
    {
        int totalEnemies = instantiatedEnemiesParent.transform.childCount;
        if (waves.Count > 0)
        {
            if (waves[(int)currentWaveIndex].MinimumAmountOfEnemies >= totalEnemies)
            {
                currentWaveIndex++;
                SpawnEnemies(currentWaveIndex);
            }
        }
    }

    public void SpawnEnemies(float waveIndex)
    {
        EnableNearbySpawnPoints();
        DisableNearbySpawnPoints();

        // Get the wave data
        WaveData wave = waves[(int)waveIndex];

        if (wave == null)
        {
            Debug.Log("No more waves");
            return;
        }

        foreach (KeyValuePair<MobType, int> mobData in wave.Mobs)
        {
            String mobType = mobData.Key.ToString();
            int spawnCount = mobData.Value;

            GameObject enemyPrefab = GetEnemyPrefabByType(mobType);

            for (int i = 0; i < spawnCount; i++)
            {
                List<Transform> activeSpawnPoints = GetActiveSpawnPoints();

                if (activeSpawnPoints.Count == 0)
                {
                    Debug.LogWarning("No active spawn points found for wave " + waveIndex);
                    return;
                }

                Transform spawnPoint = GetRandomSpawnPoint(activeSpawnPoints);
                Vector3 spawnPosition = GetRandomSpawnPosition(spawnPoint);
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, instantiatedEnemiesParent.transform);
            }
        }

        Debug.Log("Wave " + waveIndex + " spawned");
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
        waves.Add(new WaveData(1.0f, 1.0f, 1.0f, 5, new Dictionary<MobType, int>
        {
            { MobType.Blue_Skellie, 5 },
            { MobType.Gobbie, 3 }
        }));
        waves.Add(new WaveData(1.2f, 1.1f, 1.2f, 5, new Dictionary<MobType, int>
        {
            { MobType.Pumpkin_Gobbie, 8 },
            { MobType.Gobbie, 5 },
            { MobType.Skellie, 5}
        }));    
        waves.Add(new WaveData(1.5f, 1.2f, 1.5f, 5, new Dictionary<MobType, int>
        {
            { MobType.Skellie, 10 },
            { MobType.Gobbie, 8 }
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
