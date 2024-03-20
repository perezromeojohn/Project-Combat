using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerResources : MonoBehaviour
{
    public Resource resources;
    public TextMeshProUGUI coinsText;

    void Start()
    {
        resources.LoadResources();
        // coinsText.text = resources.coins.ToString();
        string coins = resources.coins.ToString();
        coinsText.text = "x " + coins;
    }

    void Update()
    {
        string coins = resources.coins.ToString();
        coinsText.text = "x " + coins;
    }
}
