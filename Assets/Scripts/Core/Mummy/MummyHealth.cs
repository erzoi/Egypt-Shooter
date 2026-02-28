using System;
using UnityEngine;

/// <summary>
/// Скрипт керування здоров'єм мумії
/// <para>Містить максимальну та поточну кількість здоров'я мумії. Реалізує метод отримання пошкодження</para>
/// </summary>
public class MummyHealth : MonoBehaviour, IDamageable
{
    // Максимальна кількість здоров'я мумії
    [SerializeField] private float baseHealth = 50;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;

    // Поточна кількість здоров'я мумії
    private float currentHealth;

    // Змінна, яка визначає жива мумія чи ні
    public bool IsDead => currentHealth <= 0;

    public Action OnDead;

    private void Awake()
    {
        currentHealth = baseHealth;
    }

    private void Start()
    {
        ApplyDifficulty();
    }

    /// <summary>
    /// Метод обробки отримання пошкодження мумією
    /// </summary>
    /// <param name="damage">Значення пошкодження</param>
    public void TakeDamage(float damage)
    {
        // Якщо мумія мертва - завершуємо виконання
        if (IsDead)
            return;

        // Віднімаємо значення пошкодження від поточного значення здоров'я
        currentHealth -= damage;

        if (currentHealth > 0)
            AudioManager.Instance.PlaySFX(hurtSound, transform.position);

        // Якщо поточне здоров'є менше, або дорівнює 0 - мумія помирає
        if (currentHealth <= 0)
            Die();
    }

    public void ApplyDifficulty()
    {
        int stage = Mathf.FloorToInt(GameTimeManager.GameTime / 60f);
        
        float healthMultiplier = 1f + stage * 0.4f;
        Mathf.Clamp(healthMultiplier, healthMultiplier, 3f);

        currentHealth = baseHealth * healthMultiplier;
    }

    /// <summary>
    /// Метод обробки смерті мумії
    /// </summary>
    private void Die()
    {
        AudioManager.Instance.PlaySFX(deathSound, transform.position);
        OnDead?.Invoke();
    }

    public bool Heal(int value)
    {
        throw new NotImplementedException();
    }
}
