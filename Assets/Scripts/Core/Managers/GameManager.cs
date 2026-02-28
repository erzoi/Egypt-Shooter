using System;
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

    public Action<GameState> OnGameStateChanged;

    // Стандартна реалізація Singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(Instance);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        StartGame();
    }

    /// <summary>
    /// Метод обробки запуску ігрового процесу
    /// </summary>
    public void StartGame()
    {
        ScoreManager.Instance.Reset();
        GameTimeManager.ResetTime();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;
        SetState(GameState.Playing);
    }

    public void PauseGame()
    {
        if (CurrentState != GameState.Playing)
            return;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
        SetState(GameState.Paused);
    }

    public void ResumeGame()
    {
        if (CurrentState != GameState.Paused)
            return;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;
        SetState(GameState.Playing);
    }

    /// <summary>
    /// Метод обробки закінчення гри
    /// </summary>
    public void GameOver()
    {
        if (CurrentState == GameState.GameOver)
            return;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
        SetState(GameState.GameOver);
    }

    private void SetState(GameState newState)
    {
        CurrentState = newState;
        OnGameStateChanged?.Invoke(newState);
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
        Paused,
        /// <summary>
        /// Кінець гри
        /// </summary>
        GameOver
    }
}
