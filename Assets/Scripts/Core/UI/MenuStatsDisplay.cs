using TMPro;
using UnityEngine;

public class MenuStatsDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timeText;

    private void OnEnable()
    {
        scoreText.text = ScoreManager.Score.ToString("N0");
        timeText.text = GameTimeManager.GetFormattedTime();
    }
}
