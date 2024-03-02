using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWave : MonoBehaviour
{
    [SerializeField]
    public List<WaveData> waves = new List<WaveData>();

    void Start()
    {
        AddWaveData(new WaveData());
    }

    // Example function to add waves (you can call this from Unity Editor or somewhere else)
    public void AddWaveData(WaveData waveData) {
        waveData.HpMultiplier = 1.0f;
        waveData.WalkSpeedMultiplier = 1.0f;
        waveData.XPMultiplier = 1.0f;
        waveData.MinMobs = 10;
        waveData.Mobs.Add("Mob1", 5);
        waveData.Mobs.Add("Mob2", 5);
        waveData.Mobs.Add("Mob3", 5);
        waves.Add(waveData);
        PrintWaveData();
    }

    // Example function to print wave data (for testing/debugging)
    public void PrintWaveData() {
        foreach (WaveData wave in waves) {
            Debug.Log("Wave:");
            Debug.Log("HP Multiplier: " + wave.HpMultiplier);
            Debug.Log("Walk Speed Multiplier: " + wave.WalkSpeedMultiplier);
            Debug.Log("XP Multiplier: " + wave.XPMultiplier);
            Debug.Log("Min Mobs: " + wave.MinMobs);
            Debug.Log("Mobs:");
            foreach (var mob in wave.Mobs) {
                Debug.Log(mob.Key + ": " + mob.Value);
            }
        }
    }
}
