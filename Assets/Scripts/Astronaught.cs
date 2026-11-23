
using UnityEngine;

public class Astronaught : MonoBehaviour
{
    public int points;


    private void Start()
    {

    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerPawn>() == null)
            return;

        if (GameManager.instance != null)
        {
            GameManager.instance.AddToScore(points);
        }

        Destroy(gameObject);
    }
}
