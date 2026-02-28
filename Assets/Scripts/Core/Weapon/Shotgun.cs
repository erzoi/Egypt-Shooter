using UnityEngine;

public class Shotgun : Weapon
{
    [Header("Stats")]
    [SerializeField] private int damagePerPellet = 25;
    [SerializeField] private int pellets = 6;
    [SerializeField] private float range = 40f;

    [Header("Ammo")]
    [SerializeField] private int maxShells = 6;

    [SerializeField] private Sprite weaponIcon;

    private int currentAmmo;
    private Animator animator;

    private bool isReloading = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentAmmo = maxShells;

        HUDController.Instance.SetActiveWeapon(this);

        NotifyWeaponSelected(weaponIcon);
        NotifyAmmoChanged(currentAmmo);
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
        if (isReloading || currentAmmo == maxShells)
            return;

        currentState = WeaponState.Reloading;
        isReloading = true;

        animator.SetTrigger("PriorToReload");
    }

    public override void Hide()
    {
        animator.SetTrigger("Hide");
        isReloading = false;
        currentState = WeaponState.Hidden;
    }

    public override void Unhide()
    {
        currentState = WeaponState.Drawing;
        animator.SetTrigger("Unhide");
        NotifyWeaponSelected(weaponIcon);
        NotifyAmmoChanged(currentAmmo);
    }

    public void OnFireEvent()
    {
        currentAmmo--;
        NotifyAmmoChanged(currentAmmo);

        if (currentAmmo <= 0)
        {
            Reload();
            return;
        }

        for (int i = 0; i < pellets; i++)
        {
            Vector3 direction = new Vector3(0.5f, 0.5f);
            direction += Random.insideUnitSphere * 0.05f;

            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

            if (Physics.Raycast(ray, out RaycastHit hit, range, hitMask))
            {
                if (hit.collider.TryGetComponent<IDamageable>(out var damageable))
                    damageable.TakeDamage(damagePerPellet);
            }
        }
    }

    public void OnPostFireEvent()
    {
        animator.SetTrigger("PostFire");
    }

    public void OnInsertShellEvent()
    {
        if (currentAmmo >= maxShells)
        {
            animator.SetTrigger("ReloadLastOne");
            return;
        }

        if (currentAmmo < maxShells)
            animator.SetTrigger("ReloadOne");
        else
            animator.SetTrigger("ReloadLastOne");
        
        currentAmmo++;

        NotifyAmmoChanged(currentAmmo);
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
