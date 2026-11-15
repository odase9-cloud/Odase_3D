
using UnityEngine;

public class astriod : MonoBehaviour
{
    public float damageAmount;

    private void Start()
    {
        GameManager gm = GameManager.instance;

        if (gm != null &&
            gm.asteriodList != null &&
            !gm.asteriodList.Contains(this))
        {
            gm.asteriodList.Add(this);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // Asteroids should not damage other asteroids.
        if (collision.GetComponent<astriod>() != null)
            return;

        Health health = collision.GetComponent<Health>();

        if (health != null)
        {
            health.TakeDamage(damageAmount);
        }
    }
}

