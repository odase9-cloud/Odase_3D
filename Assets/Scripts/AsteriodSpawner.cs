
using UnityEngine;

public class AsteriodSpawner : MonoBehaviour
{
    [Header("Asteroid Settings")]
    public GameObject asteroidPrefab;
    public int asteroidCount;

    [Header("3D Spawn Area")]
    public float spawnAreaWidth;
    public float spawnAreaHeight;
    public float spawnAreaDepth;
    public float spawnPadding;

    [Header("Player Safety")]
    public float minDistanceFromPlayer;
    public int maxAttemptsPerAsteroid;

    private Transform playerTransform;

    private void Start()
    {
        FindPlayer();
        SpawnAsteroids();
    }

    private void FindPlayer()
    {
        if (GameManager.instance != null &&
            GameManager.instance.playerController != null &&
            GameManager.instance.playerController.pawn != null)
        {
            playerTransform = GameManager.instance
                .playerController.pawn.transform;
        }

        if (playerTransform == null)
        {
            Debug.LogError(
                "Asteroid Spawner: Could not find the player!");
        }
    }

    public void SpawnAsteroids()
    {
        if (asteroidPrefab == null)
        {
            Debug.LogError(
                "Asteroid Spawner: Assign an asteroid prefab!");
            return;
        }

        if (playerTransform == null)
        {
            FindPlayer();

            if (playerTransform == null)
                return;
        }

        int spawned = 0;

        for (int i = 0; i < asteroidCount; i++)
        {
            bool foundPosition = false;
            Vector3 spawnPosition = Vector3.zero;

            for (int attempt = 0;
                 attempt < maxAttemptsPerAsteroid;
                 attempt++)
            {
                // Random position inside a 3D box around the player.
                Vector3 offset = new Vector3(
                    Random.Range(
                        -spawnAreaWidth / 2f + spawnPadding,
                         spawnAreaWidth / 2f - spawnPadding),
                    Random.Range(
                        -spawnAreaHeight / 2f + spawnPadding,
                         spawnAreaHeight / 2f - spawnPadding),
                    Random.Range(
                        -spawnAreaDepth / 2f + spawnPadding,
                         spawnAreaDepth / 2f - spawnPadding)
                );

                spawnPosition = playerTransform.position + offset;

                // Check the full 3D distance from the player.
                if (Vector3.Distance(
                    spawnPosition,
                    playerTransform.position)
                    >= minDistanceFromPlayer)
                {
                    foundPosition = true;
                    break;
                }
            }

            if (!foundPosition)
            {
                Debug.LogWarning(
                    "Could not find a safe position for asteroid " + i);
                continue;
            }

            GameObject spawnedAsteroid = Instantiate(
                asteroidPrefab,
                spawnPosition,
                Quaternion.identity
            );

            astriod asteroidScript =
                spawnedAsteroid.GetComponent<astriod>();

            if (asteroidScript != null &&
                GameManager.instance != null)
            {
                if (!GameManager.instance.asteriodList
                    .Contains(asteroidScript))
                {
                    GameManager.instance.asteriodList
                        .Add(asteroidScript);
                }
            }
            else
            {
                Debug.LogError(
                    "Spawned asteroid is missing its astriod script!");
            }

            spawned++;
        }

        Debug.Log(
            "Asteroid Spawner created " + spawned + " asteroids.");

        if (GameManager.instance != null)
        {
            GameManager.instance.FinishAsteroidSpawning();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Transform target = playerTransform;

        if (target == null &&
            GameManager.instance != null &&
            GameManager.instance.playerController != null &&
            GameManager.instance.playerController.pawn != null)
        {
            target = GameManager.instance
                .playerController.pawn.transform;
        }

        if (target == null)
            return;

        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(
            target.position,
            new Vector3(
                spawnAreaWidth,
                spawnAreaHeight,
                spawnAreaDepth
            )
        );
    }
}
