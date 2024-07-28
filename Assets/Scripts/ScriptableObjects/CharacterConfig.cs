using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "CharacterConfig")]
public class CharacterConfig : ScriptableObject
{
    public string characterName;
    public AnimatorOverrideController characterAnimator;
    public AnimatorOverrideController hairAnimator;
    public Perks initialPerk;
}

