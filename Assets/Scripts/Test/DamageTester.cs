using UnityEngine;

/// <summary>
/// Скрипт для тестування отримання пошкодження
/// </summary>
public class DamageTester : MonoBehaviour
{
    // Ціль, яка отримує пошкодження
    [SerializeField] private DamageReceiver target;
    // Кількість здоров'я, яке втрачає ціль
    [SerializeField] private int damageAmount = 10;
    // Інтервал отримання пошкодження
    [SerializeField] private float damageInterval = 1.5f;
    // Змінна для таймеру
    private float timer;

    private void Awake()
    {
        // Якщо скрипт використовується, але не вказано ціль
        if (target == null)
        {
            // Виводимо повідомлення
            Debug.LogError("DamageTarget does not implement IDamageable");
        }
    }

    private void Update()
    {
        // Якщо поточний стан гри відрізнаяється від активного, завершуємо виконання
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing)
            return;

        // Збільшуємо таймер
        timer += Time.deltaTime;

        // Якщо таймер входить в інтервал отримання пошкодження
        if (timer >= damageInterval)
        {
            // Скидаємо таймер
            timer = 0f;
            // Виконуємо отримання пошкодження у скрипті цілі
            target.TakeDamage(damageAmount);
        }
    }
}
