
using UnityEngine;

public class DeathVolume : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        Death death = collision.GetComponent<Death>();

        if (death != null)
        {
            death.Die();
        }
    }
}
