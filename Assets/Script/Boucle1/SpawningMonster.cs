using UnityEngine;

public class SpawningMonster : MonoBehaviour
{
    public GameObject monsterPrefab;     // Assigne ton prefab ici dans l'inspecteur !
    public Transform cameraTransform;    // Peut être laissé vide, auto-assigné sinon
    public float spawnDistance = 20f;
    public float travelTime = 30f;

    private GameObject monsterInstance;
    private Vector3 startPos, endPos;
    private float elapsed = 0f;
    private bool moving = false;

    void OnEnable()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main != null ? Camera.main.transform : null;

        if (cameraTransform == null || monsterPrefab == null)
        {
            enabled = false;
            return;
        }

        // Calculer la position de spawn devant la caméra
        startPos = cameraTransform.position + cameraTransform.forward * spawnDistance;
        endPos = cameraTransform.position;

        // Instancier le monstre à la bonne position et rotation
        monsterInstance = Instantiate(monsterPrefab, startPos, Quaternion.identity);
        moving = true;
    }

    void Update()
    {
        if (!moving || monsterInstance == null)
            return;

        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / travelTime);

        // Interpolation de la position
        monsterInstance.transform.position = Vector3.Lerp(startPos, endPos, t);

        // Toujours regarder la caméra
        monsterInstance.transform.LookAt(cameraTransform.position);

        // Stopper le mouvement à la fin (optionnel)
        if (t >= 1f)
            moving = false;
    }
}
