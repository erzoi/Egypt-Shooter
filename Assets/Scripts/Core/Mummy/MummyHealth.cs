using System;
using UnityEngine;

/// <summary>
/// Скрипт керування здоров'єм мумії
/// <para>Містить максимальну та поточну кількість здоров'я мумії. Реалізує метод отримання пошкодження</para>
/// </summary>
public class MummyHealth : MonoBehaviour, IDamageable
{
    // Максимальна кількість здоров'я мумії
    [SerializeField] private int maxHealth = 50;
    // Поточна кількість здоров'я мумії
    private int currentHealth;

    // Змінна, яка визначає жива мумія чи ні
    public bool IsDead => currentHealth <= 0;

    public Action OnDead;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Метод обробки отримання пошкодження мумією
    /// </summary>
    /// <param name="damage">Значення пошкодження</param>
    public void TakeDamage(int damage)
    {
        // Якщо мумія мертва - завершуємо виконання
        if (IsDead)
            return;

        // Віднімаємо значення пошкодження від поточного значення здоров'я
        currentHealth -= damage;
        Debug.Log($"Enemy took damage. HP: {currentHealth}");

        // Якщо поточне здоров'є менше, або дорівнює 0 - мумія помирає
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Метод обробки смерті мумії
    /// </summary>
    private void Die()
    {
        OnDead?.Invoke();
    }
}
