using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public static HUDController Instance { get; private set; }

    [Header("Timer")]
    [SerializeField] private TMP_Text timeText;

    [Header("Health")]
    [SerializeField] private TMP_Text healthText;

    [Header("Ammo")]
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TMP_Text ammoText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(Instance);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        timeText.text = GameTimeManager.GetFormattedTime();
    }

    public void Initialize(PlayerHealth health)
    {
        health.OnHealthChanged += UpdateHealth;
    }

    public void SetActiveWeapon(Weapon weapon)
    {
        weapon.OnAmmoChanged += UpdateAmmo;
        weapon.OnWeaponSelected += UpdateWeaponIcon;
    }

    private void UpdateHealth(int current)
    {
        healthText.text = current.ToString();
    }

    private void UpdateAmmo(int current)
    {
        ammoText.text = current.ToString();
    }

    private void UpdateWeaponIcon(Sprite icon)
    {
        weaponIcon.sprite = icon;
    }
}
