using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveData {
    public float HpMultiplier { get; set; }
    public float WalkSpeedMultiplier { get; set; }
    public float XPMultiplier { get; set; }
    public int MinMobs { get; set; }
    public Dictionary<string, int> Mobs { get; set; } // Dictionary to store mob name and count

    // Constructor
    public WaveData() {
        Mobs = new Dictionary<string, int>();
    }
}