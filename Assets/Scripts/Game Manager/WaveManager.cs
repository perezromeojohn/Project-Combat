using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public float HpMultiplier;
        public float WalkSpeedMultiplier;
        public float XPMultiplier;
        public int MinMobs;
        public Dictionary<string, int> Mobs = new Dictionary<string, int>(); // Dictionary to hold mob name and count
    }

    public List<Wave> waves = new List<Wave>();
    private int currentWaveIndex = 0;
    private int totalSpawnedMobs = 0;

    void Start()
    {
        StartNextWave();
    }

    void StartNextWave()
    {
        if (currentWaveIndex < waves.Count)
        {
            Wave currentWave = waves[currentWaveIndex];
            SpawnMobs(currentWave.Mobs);
        }
        else
        {
            Debug.Log("No more waves available.");
            // Handle end of waves
        }
    }

    void SpawnMobs(Dictionary<string, int> mobs)
    {
        foreach (KeyValuePair<string, int> pair in mobs)
        {
            string mobName = pair.Key;
            int count = pair.Value;
            // Spawn logic for each mob
            Debug.Log("Spawning " + count + " " + mobName + "(s).");
            totalSpawnedMobs += count;
        }
    }

    void Update()
    {
        // Check if the minimum number of mobs for the current wave has been reached
        if (totalSpawnedMobs >= waves[currentWaveIndex].MinMobs)
        {
            // Move to the next wave
            currentWaveIndex++;
            totalSpawnedMobs = 0; // Reset total spawned mobs for the next wave
            StartNextWave();
        }
    }
}
