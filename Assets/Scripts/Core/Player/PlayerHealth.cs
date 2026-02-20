using System;
using UnityEngine;

/// <summary>
/// Скрипт керування здоров'єм гравця
/// <para>Містить максимальну та поточну кількість здоров'я гравця. Реалізує метод отримання пошкодження</para>
/// </summary>
public class PlayerHealth : MonoBehaviour, IDamageable
{
    // Максимальна кількість здоров'я гравця
    [SerializeField] private int maxHealth = 100;

    // Поточна кількість здоров'я гравця
    public int CurrentHealth { get; private set; }
    // Змінна, яка визначає жива мумія чи ні
    public bool IsDead => CurrentHealth <= 0;

    public event Action<int> OnHealthChanged;

    private void Awake()
    {
        CurrentHealth = maxHealth;
        HUDController.Instance.Initialize(this);
        NotifyHealthChanged(CurrentHealth);
    }

    /// <summary>
    /// Метод обробки отримання пошкодження гравцем
    /// </summary>
    /// <param name="damage">Значення пошкодження</param>
    public void TakeDamage(int damage)
    {
        // Якщо мумія мертвий - завершуємо виконання
        if (IsDead)
            return;

        // Віднімаємо значення пошкодження від поточного значення здоров'я
        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);

        NotifyHealthChanged(CurrentHealth);

        // Якщо поточне здоров'є менше, або дорівнює 0 - гравець помирає
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Метод обробки смерті мумії
    /// </summary>
    private void Die()
    {
        Debug.Log("Player died");
        // Виконуємо метод обробки завершення гри
        GameManager.Instance.GameOver();
    }

    private void NotifyHealthChanged(int current)
    {
        OnHealthChanged?.Invoke(current);
    }
}
