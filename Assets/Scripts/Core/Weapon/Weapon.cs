using System;
using UnityEngine;

/// <summary>
/// Базовий абстрактний клас керування зброєю.
/// Містить стан зброї та оголошення необхідних методів.
/// </summary>
public abstract class Weapon : MonoBehaviour
{
    [Header("References")]
    // Камера гравця
    [SerializeField] protected Camera playerCamera;
    
    // Маска влучання
    [SerializeField] protected LayerMask hitMask;

    // Поточний стан зброї
    [SerializeField] protected WeaponState currentState = WeaponState.Hidden;

    public event Action<int> OnAmmoChanged; // у магазині
    public event Action<Sprite> OnWeaponSelected;

    // Абстрактне оголошення метода стрільби зброї
    public abstract void Fire();
    // Абстрактне оголошення метода перезаряджання зброї
    public abstract void Reload();
    // Абстрактне оголошення метода приховування зброї
    public abstract void Hide();
    // Абстрактне оголошення метода діставання зброї
    public abstract void Unhide();

    protected void NotifyAmmoChanged(int current)
    {
        OnAmmoChanged?.Invoke(current);
    }

    protected void NotifyWeaponSelected(Sprite icon)
    {
        OnWeaponSelected?.Invoke(icon);
    }

    // Геттер для поля поточного стану зброї
    public WeaponState GetState() => currentState;
}
