using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuFlow : MonoBehaviour
{
    public bool canPressSpaceBar = false;
    public Animator mainMenuAnimator;
    public AudioSource startSound;

    // Update is called once per frame
    void Update()
    {
        if (canPressSpaceBar && Input.GetKeyDown(KeyCode.Space))
        {
            mainMenuAnimator.SetBool("isSpaceBarPressed", true);
            canPressSpaceBar = false;
            startSound.Play();
        }
    }

    public void MoveToMainGameScene()
    {
        // Load the main game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

    public void CanPressSpaceBar()
    {
        canPressSpaceBar = true;
    }
}
