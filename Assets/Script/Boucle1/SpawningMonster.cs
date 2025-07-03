using UnityEngine;

public class SpawningMonster : MonoBehaviour
{
    public GameObject monsterPrefab;
    public Transform cameraTransform;
    public float spawnDistance = 20f;
    public float travelTime = 30f;

    private GameObject monsterInstance;
    private float elapsed = 0f;
    private bool moving = false;
    private float speed = 0f;

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
        Vector3 startPos = cameraTransform.position + cameraTransform.forward * spawnDistance;

        // Instancier le monstre à la bonne position et rotation
        monsterInstance = Instantiate(monsterPrefab, startPos, Quaternion.identity);

        // Calculer la **distance initiale**
        float distance = Vector3.Distance(startPos, cameraTransform.position);

        // Calculer la **vitesse** pour arriver en travelTime, en ligne droite
        speed = distance / travelTime;

        elapsed = 0f;
        moving = true;
    }

    void Update()
    {
        if (!moving || monsterInstance == null)
            return;

        elapsed += Time.deltaTime;

        // Direction dynamique : toujours vers la position de la caméra à chaque frame
        Vector3 direction = (cameraTransform.position - monsterInstance.transform.position).normalized;

        // Distance à avancer cette frame
        float step = speed * Time.deltaTime;

        // Avancer vers la caméra : move jusqu’à la caméra (mais jamais “traverser”)
        float distanceToTarget = Vector3.Distance(monsterInstance.transform.position, cameraTransform.position);
        if (step > distanceToTarget)
        {
            monsterInstance.transform.position = cameraTransform.position;
        }
        else
        {
            monsterInstance.transform.position += direction * step;
        }

        // Regarde toujours la caméra
        monsterInstance.transform.LookAt(cameraTransform.position);

        // Arrêt : si travelTime écoulé OU très proche de la caméra
        if (elapsed >= travelTime || distanceToTarget < 0.1f)
            moving = false;
    }
}
