using UnityEngine;

public class HealthPU : MonoBehaviour, ISpawnable
{
    [SerializeField] private AudioClip soundEffect;
    [SerializeField] private int healValue = 50;

    private ObjectSpawner spawner;

    public void Initialize(ObjectSpawner spawner)
    {
        this.spawner = spawner;
    }

    public void Pickup()
    {
        spawner?.OnObjectDestroy();
        AudioManager.Instance.PlaySFX(soundEffect, transform.position);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        DamageReceiver health = other.GetComponent<DamageReceiver>();

        if (health != null && health.Heal(healValue))
            Pickup();
    }
}
