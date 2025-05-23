using UnityEngine;

public class SpawnKeys : MonoBehaviour
{
    public static GameObject prefab;
    public static Transform keysParent;

    public static void SpawnAbovePlayer(Vector3 center, int count = 3, float radius = 1f, float minHeight = 5f, float maxHeight = 7f)
    {   
        // Détection ou création du parent
        if (keysParent == null)
        {
            GameObject parentGO = GameObject.Find("SpawnKeys");
            if (parentGO != null)
                keysParent = parentGO.transform;
            else
                Debug.LogWarning("Aucun GameObject nommé 'SpawnKeys' trouvé pour être le parent des clés !");
        }

        Debug.Log($"SpawnKeys.SpawnAbovePlayer() called, count: {count}");

        for (int i = 0; i < count; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * radius;
            float height = Random.Range(minHeight, maxHeight);
            Vector3 spawnPos = center + new Vector3(randomCircle.x, height, randomCircle.y);
            GameObject key = Object.Instantiate(prefab, spawnPos, Quaternion.identity, keysParent);
            Debug.Log($"Pluie de clé : spawned key at {spawnPos}");
        }
    }
}
