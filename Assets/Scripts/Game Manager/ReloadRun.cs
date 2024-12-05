using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadRun : MonoBehaviour
{
    public void RestartScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
