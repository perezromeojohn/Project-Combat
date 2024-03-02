using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<Transform> spawnPoints;
    public List<GameObject> enemyPrefabs;
    private GameObject player;
    public int totalEnemiesToSpawn = 90;
    public GameObject instantiatedEnemiesParent;
    public float disableRadius = 5f;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        SpawnEnemies();
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

    public void SpawnEnemies()
    {
        EnableNearbySpawnPoints();
        DisableNearbySpawnPoints();

        List<Transform> activeSpawnPoints = new List<Transform>();
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint.gameObject.activeSelf)
            {
                activeSpawnPoints.Add(spawnPoint);
            }
        }

        int enemiesPerSpawnPoint = totalEnemiesToSpawn / activeSpawnPoints.Count;
        int remainingEnemies = totalEnemiesToSpawn % activeSpawnPoints.Count;

        foreach (Transform spawnPoint in activeSpawnPoints)
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
