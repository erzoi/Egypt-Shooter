using UnityEngine;

public class WeaponAudio : MonoBehaviour
{
    [Header("Clips")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip reloadClip;
    [SerializeField] private AudioClip clockClip;

    [Header("Settings")]
    [SerializeField] private float shootVolume = 1f;
    [SerializeField] private float reloadVolume = 1f;
    [SerializeField] private float clockVolume = 1f;

    public void PlayShoot()
    {
        AudioManager.Instance.PlaySFX(shootClip, transform.position, shootVolume);
    }

    public void PlayReload()
    {
        AudioManager.Instance.PlaySFX(reloadClip, transform.position, reloadVolume);
    }

    public void PlayClock()
    {
        AudioManager.Instance.PlaySFX(clockClip, transform.position, clockVolume);
    }
}
