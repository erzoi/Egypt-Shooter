using UnityEngine;

/// <summary>
/// Скрипт керування пістолетом
/// </summary>
public class Pistol : Weapon
{
    [Header("Stats")]
    // Значення пошеодження пістолета
    [SerializeField] private int damage = 10;
    // Дистанція стрільби пістолета
    [SerializeField] private float range = 100f;
    // Швидкість стрільби пістолета (вистріли у секунду)
    [SerializeField] private float fireRate = .5f;

    [Header("Ammo")]
    // Максимальна кількість патронів у магазині
    [SerializeField] private int magazineSize = 12;
    // Кількість допо
    [SerializeField] private int reserveAmmo = 84;

    [SerializeField] private Sprite weaponIcon;

    // Поточна кількість патронів у магазині
    private int currentAmmo;

    // Таймер для виконання наступного вистрілу
    private float nextFireTime = 0f;

    // Об'єкт керування анімаціями пістолету
    private Animator animator;
    // Камера гравця
    private Camera playerCamera;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerCamera = GetComponentInParent<Camera>();
        currentAmmo = magazineSize;

        HUDController.Instance.SetActiveWeapon(this);

        NotifyWeaponSelected(weaponIcon);
        NotifyAmmoChanged(currentAmmo, reserveAmmo);
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
        if (currentState == WeaponState.Reloading || reserveAmmo <= 0 || currentAmmo == magazineSize)
            return;

        currentState = WeaponState.Reloading;

        if (currentAmmo == 0)
            animator.SetTrigger("ReloadEmpty");
        else
            animator.SetTrigger("Reload");

        reserveAmmo -= magazineSize - currentAmmo;
    }

    /// <summary>
    /// Реалізація абстрактного методу діставання для пістолета
    /// </summary>
    public override void Unhide()
    {
        currentState = WeaponState.Drawing;
        animator.SetTrigger("Unhide");
        NotifyWeaponSelected(weaponIcon);
        NotifyAmmoChanged(currentAmmo, reserveAmmo);
    }
    
    public void OnFireAnimationEvent()
    {
        currentAmmo--;

        NotifyAmmoChanged(currentAmmo, reserveAmmo);

        if (currentAmmo <= 0)
        {
            Reload();
            return;
        }

        if (Physics.Raycast(playerCamera.transform.position,
                            playerCamera.transform.forward,
                            out RaycastHit hit,
                            range))
        {
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }
    }

    public void OnAnimationFinished()
    {
        if (currentState == WeaponState.Reloading)
        {
            currentAmmo = magazineSize;
            NotifyAmmoChanged(currentAmmo, reserveAmmo);
        }
        
        currentState = WeaponState.Idle;
    }
}
