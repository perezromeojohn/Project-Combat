using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject topBar;
    [SerializeField] GameObject middleBar;
    [SerializeField] GameObject bottomBar;

    [SerializeField] TextMeshProUGUI timeElapsedText;
    [SerializeField] TextMeshProUGUI killCountText;
    [SerializeField] TextMeshProUGUI totalDamageText;

    [SerializeField] private Button retryButton;
    [SerializeField] private Button quitButton;

    [Header("Configs")]
    [SerializeField] private TimeManager timeManager;

    void Start()
    {
        retryButton.onClick.AddListener(() => {
            Retry();
        });
        quitButton.onClick.AddListener(() => {
            Quit();
        });
    }

    public void UpdateTexts()
    {
        gameOverPanel.SetActive(true);
        int minutes = Mathf.FloorToInt(timeManager.GetTimeElapsed() / 60);
        int seconds = Mathf.FloorToInt(timeManager.GetTimeElapsed() % 60);
        timeElapsedText.text = string.Format("{0}:{1:00}", minutes, seconds);
        killCountText.text = "x " + KillCount.GetTotalKills().ToString();
    }

    void Retry()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    void Quit()
    {
        Application.Quit();
    }
}
