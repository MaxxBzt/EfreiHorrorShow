using UnityEngine;

public class SpawnRealKey : MonoBehaviour
{
    public GameObject prefabToSpawn; 
    public Transform cameraRig;

    public float forwardDistance = 1.5f;
    public float heightAboveGround = 5f;
    public float spawnRadius = 1f;

    public LetterManage letterManage; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(letterManage.keysCalled);
        if (letterManage.keysCalled)
        {
            letterManage.keysCalled = false;
            SpawnKey();
        }
        
    }

    void SpawnKey()
    {
                Debug.Log("Spawn d'objet près de la caméra");
        Vector3[] directions = new Vector3[]
        {
            cameraRig.forward,
            -cameraRig.forward,
            cameraRig.right,
            -cameraRig.right
        };

        LayerMask environmentMask = 1 << 6; // Layer 6 = Environnement
        float maxRaycastDistance = 10f;
        int maxAttempts = 15;
        float verticalOffset = 0.05f;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector3 chosenDirection = directions[Random.Range(0, directions.Length)];
            Vector3 basePos = cameraRig.position + chosenDirection.normalized * forwardDistance;

            Vector2 randCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 offset = new Vector3(randCircle.x, 0, randCircle.y);

            Vector3 rayOrigin = basePos + offset + Vector3.up * heightAboveGround;

            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, maxRaycastDistance, environmentMask))
            {
                string lowerName = hit.collider.gameObject.name.ToLower();

                // ✅ Vérifie que c’est bien du sol ou une table
                if (lowerName.Contains("floor") || lowerName.Contains("table"))
                {
                    Vector3 spawnPos = hit.point + Vector3.up * verticalOffset;
                    Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
                    return;
                }
            }
        }

    }

}
