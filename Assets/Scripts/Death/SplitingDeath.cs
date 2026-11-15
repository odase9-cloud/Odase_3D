
using UnityEngine;
using UnityEngine.UI;

public class SplittingDeath : Death
{
    [Header("Splitting Settings")]
    public GameObject childAsteroidPrefab;
    public int childrenToSpawn;
    public float childScaleMultiplier;
    public float splitForce;
    public float minimumSplitScale;
    public int maxSplitDepth;
    public float spawnGap;

    [Header("Scoring")]
    public int scoreToAdd;

    [HideInInspector]
    public int splitDepth = 0;

    private bool hasDied = false;

    public override void Die()
    {
        if (hasDied)
            return;

        hasDied = true;

        if (GameManager.instance != null)
        {
            GameManager.instance.AddToScore(scoreToAdd);
        }

        if (childAsteroidPrefab != null &&
            splitDepth < maxSplitDepth &&
            transform.localScale.x > minimumSplitScale)
        {
            SplitAsteroid();
        }

        Destroy(gameObject);
    }

    private void SplitAsteroid()
    {
        Collider parentCollider = GetComponent<Collider>();

        float parentRadius = parentCollider != null
            ? parentCollider.bounds.extents.magnitude
            : 0.5f;

        for (int i = 0; i < childrenToSpawn; i++)
        {
            // Distribute children in 3D space.
            Vector3 direction = Random.onUnitSphere;

            float spawnDistance = parentRadius + spawnGap;

            Vector3 spawnPosition = transform.position
                + direction * spawnDistance;

            GameObject child = Instantiate(
                childAsteroidPrefab,
                spawnPosition,
                Quaternion.identity
            );

            Canvas healthCanvas =
                child.GetComponentInChildren<Canvas>(true);

            if (healthCanvas != null)
            {
                healthCanvas.enabled = true;
                healthCanvas.gameObject.SetActive(true);
                healthCanvas.overrideSorting = true;
                healthCanvas.sortingOrder = 100;

                Image healthImage =
                    healthCanvas.GetComponentInChildren<Image>(true);

                if (healthImage != null)
                {
                    healthImage.enabled = true;
                    healthImage.gameObject.SetActive(true);
                    healthImage.color = Color.white;
                    healthImage.fillAmount = 1f;
                }
                else
                {
                    Debug.LogError(
                        "Child asteroid Canvas has no Image!"
                    );
                }
            }
            else
            {
                Debug.LogError("Child asteroid has no Canvas!");
            }

            child.transform.localScale =
                transform.localScale * childScaleMultiplier;

            Collider childCollider =
                child.GetComponent<Collider>();

            float childRadius = childCollider != null
                ? childCollider.bounds.extents.magnitude
                : parentRadius * childScaleMultiplier;

            // Keep children separated from the parent asteroid.
            child.transform.position = transform.position
                + direction *
                (parentRadius + childRadius + spawnGap);

            // Keep the health canvas from scaling with the asteroid.
            if (healthCanvas != null)
            {
                float scale = Mathf.Max(
                    Mathf.Abs(child.transform.lossyScale.x),
                    0.001f
                );

                healthCanvas.transform.localScale =
                    Vector3.one / scale;
            }

            SplittingDeath childDeath =
                child.GetComponent<SplittingDeath>();

            if (childDeath != null)
            {
                childDeath.splitDepth = splitDepth + 1;
            }

            astriod childAsteroid =
                child.GetComponent<astriod>();

            if (childAsteroid != null &&
                GameManager.instance != null)
            {
                GameManager.instance.RegisterAsteroid(
                    childAsteroid
                );
            }

            Rigidbody rb = child.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(
                    direction * splitForce,
                    ForceMode.Impulse
                );
            }
        }
    }
}


