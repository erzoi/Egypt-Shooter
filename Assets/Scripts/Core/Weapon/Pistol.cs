using UnityEngine;

/// <summary>
/// Скрипт керування пістолетом
/// </summary>
public class Pistol : Weapon
{
    [Header("Stats")]
    // Значення пошеодження пістолета
    [SerializeField] private int damage = 35;
    // Дистанція стрільби пістолета
    [SerializeField] private float range = 100f;
    // Швидкість стрільби пістолета (вистріли у секунду)
    [SerializeField] private float fireRate = .5f;

    [Header("Ammo")]
    // Максимальна кількість патронів у магазині
    [SerializeField] private int magazineSize = 12;

    [SerializeField] private Sprite weaponIcon;

    // Поточна кількість патронів у магазині
    private int currentAmmo;

    // Таймер для виконання наступного вистрілу
    private float nextFireTime = 0f;

    // Об'єкт керування анімаціями пістолету
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentAmmo = magazineSize;

        HUDController.Instance.SetActiveWeapon(this);

        NotifyWeaponSelected(weaponIcon);
        NotifyAmmoChanged(currentAmmo);
    }

    /// <summary>
    /// Реалізація абстрактного методу стрільби для пістолета
    /// </summary>
    public override void Fire()
    {
        if (currentState != WeaponState.Idle)
            return;

        if (Time.time < nextFireTime)
            return;

        if (currentAmmo <= 0)
        {
            Reload();
            return;
        }

        currentState = WeaponState.Firing;
        nextFireTime = Time.time + 0.01f / fireRate;
        animator.SetTrigger("Fire");
    }

    /// <summary>
    /// Реалізація абстрактного методу приховування для пістолета
    /// </summary>
    public override void Hide()
    {
        currentState = WeaponState.Hidden;
        animator.SetTrigger("Hide");
    }

    /// <summary>
    /// Реалізація абстрактного методу перезаряджання для пістолета
    /// </summary>
    public override void Reload()
    {
        if (currentState == WeaponState.Reloading || currentAmmo == magazineSize)
            return;

        currentState = WeaponState.Reloading;

        if (currentAmmo == 0)
            animator.SetTrigger("ReloadEmpty");
        else
            animator.SetTrigger("Reload");
    }

    /// <summary>
    /// Реалізація абстрактного методу діставання для пістолета
    /// </summary>
    public override void Unhide()
    {
        currentState = WeaponState.Drawing;
        animator.SetTrigger("Unhide");
        NotifyWeaponSelected(weaponIcon);
        NotifyAmmoChanged(currentAmmo);
    }
    
    public void OnFireAnimationEvent()
    {
        currentAmmo--;
        NotifyAmmoChanged(currentAmmo);

        if (currentAmmo <= 0)
        {
            Reload();
            return;
        }

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if (Physics.Raycast(ray, out RaycastHit hit, range, hitMask))
        {
            if (hit.collider.TryGetComponent<IDamageable>(out var damageable))
                damageable.TakeDamage(damage);
        }
    }

    public void OnAnimationFinished()
    {
        if (currentState == WeaponState.Reloading)
        {
            currentAmmo = magazineSize;
            NotifyAmmoChanged(currentAmmo);
        }
        
        currentState = WeaponState.Idle;
    }
}
