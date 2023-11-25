using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerksManager : MonoBehaviour
{
    // public List<string> availableSkills; // Populate this list in the Inspector with skill names
    // public List<string> availableBuffs; // Populate this list in the Inspector with buff names
    

    public GameObject levelUpWindow;
    
    public void OpenLevelUpWindow()
    {
        levelUpWindow.SetActive(true);
        // also stop the game time
        Time.timeScale = 0;
        Debug.Log("Level up window opened");
    }
}
