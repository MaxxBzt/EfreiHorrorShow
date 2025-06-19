using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting; // Nécessaire pour TMP_Text

public class IntroSequenceVR : MonoBehaviour
{
    [Header("Effets d'intro")]
    public GameObject blinkEffect;  
    public GameObject introText;

    [Header("Spawn d'objet")]
    public GameObject prefabToSpawn;
    public Transform cameraRig;
    public float forwardDistance = 1f;
    public float heightAboveGround = 5f;
    public float spawnRadius = .5f;

    void Start()
    {
        blinkEffect.SetActive(false);
        introText.SetActive(false);

        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        blinkEffect.SetActive(true);

        Material mat = blinkEffect.GetComponent<MeshRenderer>().material;

        float duration = 6f;
        float t = 0f;

        while (t < duration)
        {
            mat.SetFloat("_time", t+1);
            t += Time.deltaTime;
            yield return null;
            mat.SetFloat("_width", Mathf.Lerp(.5f,.8f,t/duration));
        }

        blinkEffect.SetActive(false);

        introText.SetActive(true);
        yield return new WaitForSeconds(10f);

        TMP_Text tmp = introText.GetComponent<TMP_Text>();
        if (tmp != null)
        {
            yield return StartCoroutine(FadeOutText(tmp, 2f));
        }
        yield return new WaitForSeconds(5f);
        introText.SetActive(false);
        SpawnObjectNearCamera();
    }

    void SpawnObjectNearCamera()
    {
        Debug.Log("Spawn d'objet près de la caméra");
        Vector3[] directions = new Vector3[]
        {
            cameraRig.forward,
            -cameraRig.forward,
            cameraRig.right,
            -cameraRig.right
        };

        LayerMask environmentMask = 1<<6;// Layer 6 = Environnement
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
            Debug.DrawRay(rayOrigin, Vector3.down * maxRaycastDistance, Color.red, 2f);

            Debug.Log($"{attempt} : {cameraRig.position} : {rayOrigin}");

            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit))
            {
                string lowerName = hit.collider.gameObject.name.ToLower();
                Debug.Log("touché");
                // ✅ Vérifie que c’est bien du sol ou une table
                if (lowerName.Contains("floor") || lowerName.Contains("table"))
                {
                    Vector3 spawnPos = hit.point + Vector3.up * verticalOffset;
                    Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
                    return;
                }
            }

        }

        Debug.LogWarning("Aucune surface sol/table trouvée pour spawner l'objet.");
    }




    IEnumerator FadeOutText(TMP_Text textElement, float duration)
    {
        float elapsedTime = 0f;
        Color originalColor = textElement.color;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            textElement.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        textElement.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }
}