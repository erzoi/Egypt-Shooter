using System;
using UnityEngine;

/// <summary>
/// Базовий абстрактний клас керування зброєю.
/// Містить стан зброї та оголошення необхідних методів.
/// </summary>
public abstract class Weapon : MonoBehaviour
{
    // Поточний стан зброї
    protected WeaponState currentState = WeaponState.Hidden;

    public event Action<int, int> OnAmmoChanged; // у магазині, загалом
    public event Action<Sprite> OnWeaponSelected;

    // Абстрактне оголошення метода стрільби зброї
    public abstract void Fire();
    // Абстрактне оголошення метода перезаряджання зброї
    public abstract void Reload();
    // Абстрактне оголошення метода приховування зброї
    public abstract void Hide();
    // Абстрактне оголошення метода діставання зброї
    public abstract void Unhide();

    protected void NotifyAmmoChanged(int current, int reserve)
    {
        OnAmmoChanged?.Invoke(current, reserve);
    }

    protected void NotifyWeaponSelected(Sprite icon)
    {
        OnWeaponSelected?.Invoke(icon);
    }

    // Геттер для поля поточного стану зброї
    public WeaponState GetState() => currentState;
}
