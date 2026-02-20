using UnityEngine;

/// <summary>
/// Скрипт керування переміщенням мумії
/// <para>Містить ціль, до якої йде мумія, швидкість її руху та дистанція від цілі, на якій мумія зупиняється</para>
/// </summary>
public class MummyMovement : MonoBehaviour
{
    // Ціль, до якої рухається мумія
    [SerializeField] private Transform target;
    // Швидкість пересування мумії
    [SerializeField] private float moveSpeed = 3f;
    // Дистанція зупинки мумії від цілі
    [SerializeField] private float stopDistance = 2f;

    private void Update()
    {
        // Якщо поточний стан гри відрізнаяється від активного, завершуємо виконання
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing)
            return;
        // Якщо не вказано ціль, завершуємо виконання
        if (target == null)
            return;

        // Визначаємо напрямок руху
        Vector3 direction = target.position - transform.position;
        // Скидуємо рух по вертикалі
        direction.y = 0f;

        // Визначаємо дистанцію до цілі
        float distance = direction.magnitude;

        // Зупиняємо пересування, коли досягнуто дистанцію зупинки
        if (distance <= stopDistance)
            return;

        // Визначаємо вектор пересування мумії
        Vector3 move = direction.normalized * moveSpeed * Time.deltaTime;
        // Виконуємо переміщення мумії
        transform.position += move;

        // Обертаємо мумію у напрямку руху
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
