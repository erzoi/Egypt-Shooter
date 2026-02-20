using System.Collections;
using UnityEngine;

/// <summary>
/// Скрипт керування ігровим процесом
/// <para></para>
/// </summary>
public class GameManager : MonoBehaviour
{
    // Singleton екземпляр менеджеру гри
    public static GameManager Instance { get; private set; }
    // Поточний стан гри
    public GameState CurrentState { get; private set; }

    // Стандартна реалізація Singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(Instance);
            return;
        }

        Instance = this;
        
        // Не знищуэмо екземпляр менеджеру гри при зміні сцен
        DontDestroyOnLoad(Instance);
    }

    private void Start()
    {
        StartGame();
    }

    private IEnumerator TestGameOver()
    {
        yield return new WaitForSeconds(2);
    }

    private void Update()
    {
        TestGameOver();
    }

    /// <summary>
    /// Метод обробки запуску ігрового процесу
    /// </summary>
    public void StartGame()
    {
        // Змінюємо стан гри
        CurrentState = GameState.Playing;
        Debug.Log("Game Start");
    }

    /// <summary>
    /// Метод обробки закінчення гри
    /// </summary>
    public void GameOver()
    {
        // Якщо стан гри вже встановлено як "Завершена", завершуємо виконання
        if (CurrentState == GameState.GameOver)
            return;

        // Змінюємо стан гри
        CurrentState = GameState.GameOver;
        Debug.Log("Game Over");
    }

    /// <summary>
    /// Ігрові стани
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// Активна гра
        /// </summary>
        Playing,
        /// <summary>
        /// Кінець гри
        /// </summary>
        GameOver
    }
}
