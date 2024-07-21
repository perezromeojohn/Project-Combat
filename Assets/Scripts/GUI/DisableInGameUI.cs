using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableInGameUI : MonoBehaviour
{
    [SerializeField] GameObject topBar;
    [SerializeField] GameObject bottomBar;

    public void DisableUI()
    {
        topBar.SetActive(false);
        bottomBar.SetActive(false);
    }
}
