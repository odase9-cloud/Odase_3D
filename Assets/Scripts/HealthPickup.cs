
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float healAmount;

    public void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponentInParent<Health>();

        if (health == null)
            return;

        if (health.currentHealth >= health.maxHealth)
            return;

        health.Heal(healAmount);

        Destroy(gameObject);
    }
}
