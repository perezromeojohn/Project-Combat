using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitStats : MonoBehaviour
{
    public PlayerStats initPlayerStats;
    public PlayerStats inGamePlayerStats;
    void Start()
    {
        // set initial player stats to in game player stats
        foreach (var stat in initPlayerStats.GetType().GetFields())
        {
            stat.SetValue(inGamePlayerStats, stat.GetValue(initPlayerStats));
        }
    }

    void OnApplicationQuit()
    {
        foreach (var stat in inGamePlayerStats.GetType().GetFields())
        {
            stat.SetValue(inGamePlayerStats, 0);
        }
    }
}
