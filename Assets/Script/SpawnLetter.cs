using UnityEngine;

public class SpawnLetter : MonoBehaviour
{
    public static GameObject prefab;
    public static LayerMask environmentMask = 1 << 6; // Layer "Environnement"

    public static void Spawn(Vector3 origin, float radius = 2f, float heightAbove = 5f)
    {
        Debug.Log("SpawnLetter.Spawn() called");    
        Vector2 randomCircle = Random.insideUnitCircle * radius;
        Vector3 offset = new Vector3(randomCircle.x, 0, randomCircle.y);
        Vector3 rayOrigin = origin + offset + Vector3.up * heightAbove;

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 10f, environmentMask))
        {
            Debug.Log("Hit detected: " + hit.collider.gameObject.name);
            string name = hit.collider.gameObject.name.ToLower();
            if (name.Contains("floor") || name.Contains("table"))
            {
                Vector3 spawnPos = hit.point + Vector3.up * 0.05f;
                Object.Instantiate(prefab, spawnPos, Quaternion.identity);
                return;
            }
        }

    }
}
