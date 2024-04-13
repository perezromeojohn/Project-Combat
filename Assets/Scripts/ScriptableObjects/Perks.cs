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
    public bool isSkill; // if true, then it's a skill, if false, then it's a buff
    public float perkCooldown;
}
