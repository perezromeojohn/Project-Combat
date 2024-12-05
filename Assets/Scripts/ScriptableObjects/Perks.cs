using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Perks")]

public class Perks : ScriptableObject
{
    [Header("Perk Information")]
    public string perkName;
    public string perkDisplayName;
    public string[] perkDescriptions;
    public float perkCooldown;
    public bool uniquePerk;
}