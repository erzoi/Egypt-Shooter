using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using System;

/// <summary>
/// Скрит, який відповідає вза зброю гравця
/// <para>Визначає початкову зброю. Містить поточну зброю та її події</para>
/// </summary>
public class PlayerWeaponHandler : MonoBehaviour
{
    // Зброя, яку гравець може обрати
    [SerializeField] private Weapon[] weapons;

    public event Action<Weapon> OnWeaponChanged;

    // Індекс поточної зброї
    private int currentIndex = 0;
    // Поточна зброя гравця
    private Weapon currentWeapon;
    // Флаг зміни зброї
    private bool isSwitching = false;

    private void Start()
    {
        // Деаутивуємо усю зброю
        for (int i = 0; i < weapons.Length; i++)
            weapons[i].gameObject.SetActive(false);

        // Встановлюємо поточну зброю
        currentWeapon = weapons[currentIndex];
        // Активуємо поточну зброю
        currentWeapon.gameObject.SetActive(true);
        // Вмикаємо анімацію діставання поточної зброї
        currentWeapon.Unhide();

        OnWeaponChanged?.Invoke(currentWeapon);
    }
    /// <summary>
    /// Подія стрільби
    /// </summary>
    public void OnFire(InputAction.CallbackContext context)
    {
        // Вмикаємо анімацію стрільби поточної зброї
        if (context.performed)
            currentWeapon?.Fire();
    }
    /// <summary>
    /// Подія перезаряджання
    /// </summary>
    public void OnReload(InputAction.CallbackContext context)
    {
        // Вмикаємо анімацію перезаряджання поточної зброї
        if (context.performed)
            currentWeapon?.Reload();
    }
    /// <summary>
    /// Подія зміни зброї
    /// </summary>
    public void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        if (!context.performed || isSwitching)
            return;

        StartCoroutine(SwitchWeapon());
    }
    /// <summary>
    /// Процес зміни зброї
    /// </summary>
    private IEnumerator SwitchWeapon()
    {
        isSwitching = true;

        currentWeapon.Hide();

        yield return new WaitForSeconds(0.4f); // длительность Hide

        currentWeapon.gameObject.SetActive(false);

        currentIndex++;
        if (currentIndex >= weapons.Length)
            currentIndex = 0;

        currentWeapon = weapons[currentIndex];
        currentWeapon.gameObject.SetActive(true);

        currentWeapon.Unhide();

        OnWeaponChanged?.Invoke(currentWeapon);

        yield return new WaitForSeconds(0.4f); // длительность Unhide

        isSwitching = false;

    }
}
