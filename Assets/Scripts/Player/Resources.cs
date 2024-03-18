using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resources : MonoBehaviour
{
    public float coins;
    public float gems;

    // playerprefs this
    private void SaveResources()
    {
        PlayerPrefs.SetFloat("Coins", coins);
        PlayerPrefs.SetFloat("Gems", gems);
    }

    public void LoadResources()
    {
        coins = PlayerPrefs.GetFloat("Coins");
        gems = PlayerPrefs.GetFloat("Gems");
    }

    public void IncrementCoins(float value)
    {
        coins += value;
        SaveResources();
    }

    public void IncrementGems(float value)
    {
        gems += value;
        SaveResources();
    }
}
