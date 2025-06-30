using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting; // Nécessaire pour TMP_Text
using UnityEngine.UI;


public class IntroSequenceVR : MonoBehaviour
{
    [Header("Effets d'intro")]
    public GameObject blinkEffect;
    public GameObject introText;

    [Header("Spawn d'objet")]
    public GameObject prefabToSpawn;
    public Transform cameraRig;
    public float forwardDistance = .5f;
    public float heightAboveGround = 5f;
    public float spawnRadius = .25f;
    public GameObject PanelDark;
    public AudioClip WeirdSound;
    public AudioSource WeirdSoundSource;
    public GameObject Monster;
    public AudioSource whatwasthatdream;
    public AudioSource whatwasthatbook;

    void Start()
    {
        if (WeirdSoundSource != null && WeirdSound != null)
        {
            WeirdSoundSource.clip = WeirdSound;
        }
        PanelDark.SetActive(true);
        blinkEffect.SetActive(false);
        introText.SetActive(false);
        Monster.SetActive(false);


        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        PanelDark.SetActive(true);

        /*TO DO : ajouter un son d'intro */
        //WeirdSoundSource.Play();
        yield return new WaitForSeconds(4f);
        Image panelImage = PanelDark.transform.GetChild(0).GetComponent<Image>();
        if (panelImage != null)
            yield return StartCoroutine(FadeOutPanel(panelImage, 3f)); // 5 secondes de fade
        else
        PanelDark.SetActive(false);
        
        Debug.Log("PanelDark désactivé");
        //Monster appear
        Monster.SetActive(true);
        /*TO DO : ajouter un son de monstre */
        yield return new WaitForSeconds(1f);
        Monster.SetActive(false);
        
        //Start intro
        blinkEffect.SetActive(true);

        Material mat = blinkEffect.GetComponent<MeshRenderer>().material;

        float duration = 6f;
        float t = 0f;

        while (t < duration)
        {
            mat.SetFloat("_time", t + 1);
            t += Time.deltaTime;
            yield return null;
            mat.SetFloat("_width", Mathf.Lerp(.5f, .8f, t / duration));
        }

        blinkEffect.SetActive(false);

        introText.SetActive(true);
        whatwasthatdream.Play();
        yield return new WaitForSeconds(10f);

        TMP_Text tmp = introText.GetComponent<TMP_Text>();
        if (tmp != null)
        {
            yield return StartCoroutine(FadeOutText(tmp, 2f));
        }
        yield return new WaitForSeconds(5f);
        introText.SetActive(false);
        SpawnObjectNearCamera();
        yield return new WaitForSeconds(5f);
        whatwasthatbook.Play();
    }

    IEnumerator FadeOutPanel(Image panel, float duration)
    {
        float elapsed = 0f;
        Color startColor = panel.color;
        Color endColor = startColor;
        endColor.a = 0f;

        while (elapsed < duration)
        {
            panel.color = Color.Lerp(startColor, endColor, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        panel.color = endColor;
        panel.gameObject.SetActive(false);
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

        LayerMask environmentMask = 1 << 6;// Layer 6 = Environnement
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