using UnityEngine;

public class WeaponVFX : MonoBehaviour
{
    [Header("Muzzle Flash")]
    [SerializeField] private ParticleSystem muzzleFlash;

    public void PlayMuzzleFlash()
    {
        if (muzzleFlash == null)
            return;

        muzzleFlash.Play();
    }
}
