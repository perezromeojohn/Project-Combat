using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveData
{
    public float HealthMultiplier; // Multiplier for enemy health
    public float WalkSpeedMultiplier; // Multiplier for enemy walk speed
    public float XPMultiplier; // Multiplier for XP gained from defeating enemies
    public float MinimumAmountOfEnemies; // Minimum amount of enemies required to spawn the next wave
    public Dictionary<MobType, int> Mobs; // Dictionary mapping MobType to its spawn count

    // add the MinAmountOfEnemies to the constructor
    public WaveData(float healthMultiplier, float walkSpeedMultiplier, float xpMultiplier, float minAmountOfEnemies, Dictionary<MobType, int> mobs)
    {
        HealthMultiplier = healthMultiplier;
        WalkSpeedMultiplier = walkSpeedMultiplier;
        XPMultiplier = xpMultiplier;
        MinimumAmountOfEnemies = minAmountOfEnemies;
        Mobs = mobs;
    }
}

public enum MobType
{
    Pumpkin_Gobbie,
    Skellie,
    Blue_Skellie,
    Gobbie,
}