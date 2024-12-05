using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] TextMeshProUGUI timeElapsedText;
    [SerializeField] TextMeshProUGUI killCountText;
    [SerializeField] TextMeshProUGUI totalDamageText;
    [SerializeField] GameObject resumeGameButton;
    [SerializeField] GameObject gameOverTextHolder;


    void Start()
    {

    }

    public void UpdateTexts()
    {
        pausePanel.SetActive(true);
        gameOverTextHolder.SetActive(true);
        resumeGameButton.SetActive(false);
        int minutes = Mathf.FloorToInt(GlobalTimeManager.Instance.GetTimeElapsed() / 60);
        int seconds = Mathf.FloorToInt(GlobalTimeManager.Instance.GetTimeElapsed() % 60);
        killCountText.text = "Total Kills: " + KillCount.GetTotalKills().ToString();
        timeElapsedText.text = "Time Elapsed: " + string.Format("{0}:{1:00}", minutes, seconds);
    }
}
