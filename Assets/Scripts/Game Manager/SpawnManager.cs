using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<Transform> spawnPoints;
    public List<GameObject> enemyPrefabs;
    public int totalEnemiesToSpawn = 90;
    public GameObject instantiatedEnemiesParent;

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        int enemiesPerSpawnPoint = totalEnemiesToSpawn / spawnPoints.Count;
        int remainingEnemies = totalEnemiesToSpawn % spawnPoints.Count;

        foreach (Transform spawnPoint in spawnPoints)
        {
            int enemiesToSpawn = enemiesPerSpawnPoint;

            if (remainingEnemies > 0)
            {
                enemiesToSpawn++;
                remainingEnemies--;
            }

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                GameObject enemyPrefab = GetRandomEnemyPrefab();
                Vector3 spawnPosition = GetRandomSpawnPosition(spawnPoint);
                GameObject instantiatedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, instantiatedEnemiesParent.transform);
            }
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
