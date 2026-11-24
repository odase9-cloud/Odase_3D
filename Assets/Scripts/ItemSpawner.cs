
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Health Pack Settings")]
    public GameObject Prefab;
    public int ItemCount;

    [Header("3D Spawn Area")]
    public float spawnAreaWidth;
    public float spawnAreaHeight;
    public float spawnAreaDepth;
    public float spawnPadding;

    [Header("Player Safety")]
    public float minDistanceFromPlayer;
    public int maxAttemptsPerItem;

    private Transform playerTransform;

    private void Start()
    {
        FindPlayer();
        SpawnItems();
    }

    private void FindPlayer()
    {
        if (GameManager.instance != null &&
            GameManager.instance.playerController != null &&
            GameManager.instance.playerController.pawn != null)
        {
            playerTransform =
                GameManager.instance.playerController.pawn.transform;
        }

        if (playerTransform == null)
        {
            Debug.LogError(
                "Item Spawner: Could not find the player!");
        }
    }

    public void SpawnItems()
    {
        if (Prefab == null)
        {
            Debug.LogError(
                "Item Spawner: Assign a health pack prefab!");
            return;
        }

        if (playerTransform == null)
        {
            FindPlayer();

            if (playerTransform == null)
                return;
        }

        int spawned = 0;

        for (int i = 0; i < ItemCount; i++)
        {
            bool foundPosition = false;
            Vector3 spawnPosition = Vector3.zero;

            for (int attempt = 0;
                 attempt < maxAttemptsPerItem;
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

                // Keep Items away from the player.
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
                    "Could not find a safe position for Items " + i);
                continue;
            }

            Instantiate(
                Prefab,
                spawnPosition,
                Quaternion.identity
            );

            spawned++;
        }

        Debug.Log(
            "Item Spawner created " + spawned + " Items.");
    }

    private void OnDrawGizmosSelected()
    {
        Transform target = playerTransform;

        if (target == null &&
            GameManager.instance != null &&
            GameManager.instance.playerController != null &&
            GameManager.instance.playerController.pawn != null)
        {
            target =
                GameManager.instance.playerController.pawn.transform;
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
