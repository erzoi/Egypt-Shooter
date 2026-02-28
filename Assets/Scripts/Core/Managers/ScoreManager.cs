using System.Globalization;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public static int Score { get; private set; }

    [SerializeField] private TMP_Text scoreText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Reset()
    {
        Score = 0;
        UpdateUI();
    }

    public void AddScore(int baseReward)
    {
        int finalReward = CalculateReward(baseReward);

        Score += finalReward;

        UpdateUI();
    }

    int CalculateReward(int baseReward)
    {
        float difficulty = GameTimeManager.GameTime / 60f;

        float multiplier = 1f + difficulty * 0.5f;

        return Mathf.RoundToInt(baseReward * multiplier);
    }

    void UpdateUI()
    {
        scoreText.text = FormatScore(Score);
    }

    string FormatScore(int value)
    {
        return value.ToString("N0", CultureInfo.InvariantCulture)
                    .Replace(",", ".");
    }
}
