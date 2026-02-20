using UnityEngine;

/// <summary>
/// Скрипт, який відповідає за отримання пошкодження
/// <para>Сутність без даного скрипта не зможе отримувати пошкодження</para>
/// </summary>
public class DamageReceiver : MonoBehaviour, IDamageable
{
    // Компонент сутності, який відповідає за керування її здоров'єм
    [SerializeField] private MonoBehaviour healthComponent;

    // Компонент, який реалізує інтерфейс отримання пошкодження
    private IDamageable damageable;

    private void Awake()
    {
        // Зберігаємо компонент керування здоров'єм, у якості отримувача пошкоджень
        damageable = healthComponent as IDamageable;

        // Якщо перетворення не виконано
        if (damageable == null)
        {
            // Це означає, що компонент не реалізує інтерфейс отримання пошкодження
            Debug.LogError("Assigned component does not implement IDamageable");
        }
    }

    /// <summary>
    /// Метод, який викликається при отриманні пошкоджень
    /// </summary>
    /// <param name="damage">Значення пошкодження</param>
    public void TakeDamage(int damage)
    {
        // Ціль отримує пошкодження
        damageable?.TakeDamage(damage);

        // Конструкція ?. використовується для запобігання null-reference помилок
        // у випадку, коли компонент керування здоров'єм не реалізує інтерфейс отримання пошкоджень
    }
}
