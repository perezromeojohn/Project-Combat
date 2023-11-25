using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkCollect : MonoBehaviour
{
    public GameObject levelUpWindow;

    public void ResumeGame()
    {
        Time.timeScale = 1;
        levelUpWindow.SetActive(false);
        // get this game object's name and Debug.Log it
        Debug.Log(gameObject.name);
    }
}
