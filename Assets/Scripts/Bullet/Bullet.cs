
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime;
    public float forceAmount;
    public Rigidbody rb;
    public float damageAmount;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(transform.forward * forceAmount, ForceMode.Impulse);
        }

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider collision)
    {
        Health health = collision.GetComponent<Health>();

        if (health != null)
        {
            health.TakeDamage(damageAmount);
            Destroy(gameObject);
            return;
        }

        Death death = collision.GetComponent<Death>();

        if (death != null)
        {
            death.Die();
            Destroy(gameObject);
        }
    }
}
