using UnityEngine;
using TMPro;

public class KillCount : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI killCountText;
    public int totalKills = 0;

    private static KillCount instance;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        killCountText.text = "x " + totalKills.ToString();
        UpdateKillCountText();
    }

    public static void AddKill()
    {
        if (instance != null)
        {
            instance.totalKills++;
            instance.UpdateKillCountText();
        }
    }

    private void UpdateKillCountText()
    {
        if (killCountText != null)
        {
            killCountText.text = "x " + totalKills.ToString();
        }
    }

    public static float GetTotalKills()
    {
        if (instance != null)
        {
            return instance.totalKills;
        }
        return 0;
    }
}