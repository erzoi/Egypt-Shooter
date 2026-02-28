using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    public static float GameTime { get; private set; }

    void Update()
    {
        // якщо поточний стан гри в≥др≥зна€Їтьс€ в≥д активного, завершуЇмо виконанн€
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing)
            return;

        GameTime += Time.deltaTime;
    }

    public static void ResetTime()
    {
        GameTime = 0f;
    }

    public static string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(GameTime / 60f);
        int seconds = Mathf.FloorToInt(GameTime % 60f);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
