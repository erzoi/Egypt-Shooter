using UnityEngine;

/// <summary>
/// Скрипт атаки мумії
/// <para>Виконує обробку атаки мумії</para>
/// <para>Коли мумія підходить на відповідну дистанцію до гравця - вона
/// атакує його з визначеним значенням пошкодження та часом відновлення</para>
/// </summary>
public class MummyAttack : MonoBehaviour
{
    // Ціль для атакування
    [SerializeField] private DamageReceiver target;
    // Дистанциія атаки
    [SerializeField] private float attackDistance = 2.5f;
    // Значення пошкодження
    [SerializeField] private int damage = 10;
    // Час відновлення атаки
    [SerializeField] private float attackCooldown = 3f;
    // Змінна для таймеру
    private float attackTimer;

    private void Awake()
    {
        // Якщо не вказано ціль - виводимо повідомлення
        if (target == null)
        {
            Debug.LogError("DamageTarget does not implement IDamageable");
        }
    }

    private void Update()
    {
        // Якщо поточний стан гри відрізнаяється від активного, завершуємо виконання
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing)
            return;

        // Якщо не вказано ціль, завершуємо виконання
        if (target == null)
            return;

        // Збільшуємо таймер
        attackTimer += Time.deltaTime;
        // Визначаємо дистанцію до цілі
        float distance = Vector3.Distance(transform.position, target.transform.position);

        // Якщо ціль у зоні досяжності мумії та вона може атакувати
        if (distance <= attackDistance && attackTimer >= attackCooldown)
        {
            // Скидужмо час відновлення атаки
            attackTimer = 0f;
            // Ціль отримує пошкодження
            target.TakeDamage(damage);
        }
    }
}
