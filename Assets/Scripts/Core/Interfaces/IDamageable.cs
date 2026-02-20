/// <summary>
/// Інтерфейс для сутностей, які можуть отримувати пошкодження
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// Метод отримання пошкодження
    /// </summary>
    /// <param name="damage">Значення пошкодження</param>
    void TakeDamage(int damage);
}
