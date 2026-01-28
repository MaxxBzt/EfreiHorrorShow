using UnityEngine;


public class SpawnRealkey : MonoBehaviour
{
    public static GameObject prefab;
    public static LayerMask environmentMask = 1 << 6;
    private static bool hasSpawned = false;

    public static void Spawn(Vector3 center, float radius = 0.5f, float minHeight = 2f, float maxHeight = 3f, int maxAttempts = 10)
    {
        if (hasSpawned)
        {
            Debug.Log("Key has already spawned!");
            return;
        }

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * radius;
            Vector3 offset = new Vector3(randomCircle.x, 0, randomCircle.y);
            Vector3 rayOrigin = center + offset + Vector3.up * maxHeight;

            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 10f, environmentMask))
            {
                string name = hit.collider.gameObject.name.ToLower();
                if (name.Contains("floor") || name.Contains("table"))
                {
                    Vector3 spawnPos = hit.point + Vector3.up * 0.05f;
                    Object.Instantiate(prefab, spawnPos, Quaternion.identity);
                    Debug.Log($"Spawned prefab at {spawnPos} on attempt {attempt + 1}");
                    hasSpawned = true; // Empêche d’en re-spawn d’autres
                    return;
                }
            }
        }
        Debug.LogWarning("Failed to spawn key in the allowed zone after max attempts.");
    }
}
