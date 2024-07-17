 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Stat Upgrade")]
public class StatUpgrade : ScriptableObject
{
    [Header("Stat Information")]
    public string statName;
    public string statDisplayName;
    public string[] statDescriptions;
    public float baseIncreaseAmount;
}
