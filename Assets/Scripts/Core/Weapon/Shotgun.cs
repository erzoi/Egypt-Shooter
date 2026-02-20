using UnityEngine;

public class Shotgun : Weapon
{
    [Header("Stats")]
    [SerializeField] private int damagePerPellet = 8;
    [SerializeField] private int pellets = 6;
    [SerializeField] private float range = 40f;

    [Header("Ammo")]
    [SerializeField] private int maxShells = 6;
    [SerializeField] private int reserveAmmo = 42;

    [SerializeField] private Sprite weaponIcon;

    private int currentAmmo;
    private Animator animator;
    private Camera playerCamera;

    private bool isReloading = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerCamera = GetComponentInParent<Camera>();
        currentAmmo = maxShells;

        HUDController.Instance.SetActiveWeapon(this);

        NotifyWeaponSelected(weaponIcon);
        NotifyAmmoChanged(currentAmmo, reserveAmmo);
    }

    public override void Fire()
    {
        if (currentState != WeaponState.Idle)
            return;

        if (currentAmmo <= 0)
        {
            Reload();
            return;
        }

        currentState = WeaponState.Firing;
        animator.SetTrigger("Fire");
    }

    public override void Reload()
    {
        if (isReloading || reserveAmmo <= 0 || currentAmmo == maxShells)
            return;

        currentState = WeaponState.Reloading;
        isReloading = true;

        animator.SetTrigger("PriorToReload");
    }

    public override void Hide()
    {
        animator.SetTrigger("Hide");
        currentState = WeaponState.Hidden;
    }

    public override void Unhide()
    {
        currentState = WeaponState.Drawing;
        animator.SetTrigger("Unhide");
        NotifyWeaponSelected(weaponIcon);
        NotifyAmmoChanged(currentAmmo, reserveAmmo);
    }

    public void OnFireEvent()
    {
        currentAmmo--;

        if (currentAmmo <= 0)
        {
            Reload();
            return;
        }

        for (int i = 0; i < pellets; i++)
        {
            Vector3 direction = playerCamera.transform.forward;
            direction += Random.insideUnitSphere * 0.05f;

            if (Physics.Raycast(playerCamera.transform.position,
                                direction,
                                out RaycastHit hit,
                                range))
            {
                IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                damageable?.TakeDamage(damagePerPellet);
            }
        }

        NotifyAmmoChanged(currentAmmo, reserveAmmo);
    }

    public void OnPostFireEvent()
    {
        animator.SetTrigger("PostFire");
    }

    public void OnInsertShellEvent()
    {
        if (reserveAmmo <= 0 || currentAmmo >= maxShells)
        {
            animator.SetTrigger("ReloadLastOne");
            return;
        }

        reserveAmmo--;

        if (currentAmmo < maxShells && reserveAmmo > 0)
            animator.SetTrigger("ReloadOne");
        else
            animator.SetTrigger("ReloadLastOne");
        
        currentAmmo++;

        NotifyAmmoChanged(currentAmmo, reserveAmmo);
    }

    public void OnReloadFinished()
    {
        isReloading = false;
        currentState = WeaponState.Idle;
    }

    public void OnAnimationFinished()
    {
        currentState = WeaponState.Idle;
    }
}
