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
    public GameObject Introduction;

    [Header("Spawn d'objet")]
    public GameObject prefabToSpawn;
    public Transform cameraRig;
    public float forwardDistance = .5f;
    public float heightAboveGround = 5f;
    public float spawnRadius = .25f;
    public GameObject PanelDark;
    public AudioSource VoiceBeginning;
    public AudioSource WeirdSoundSource;
    public GameObject Monster;
    public AudioSource whatwasthatdream;
    public AudioSource whatwasthatbook;
    public AudioSource BaseballVoice;
    public AudioSource Stinger;

    public AudioSource Ambiant;

    void Start()
    {

        PanelDark.SetActive(true);
        blinkEffect.SetActive(false);
        introText.SetActive(false);
        Monster.SetActive(false);
        Introduction.SetActive(false);


        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        //Dark panel activation (vue fermée)
        PanelDark.SetActive(true);

        // son d'ambiance + voix bizarres
        VoiceBeginning.Play();
        WeirdSoundSource.Play();
        yield return new WaitForSeconds(14f);
        Image panelImage = PanelDark.transform.GetChild(0).GetComponent<Image>();
        if (panelImage != null)
            yield return StartCoroutine(FadeOutPanel(panelImage, 3f)); // 5 secondes de fade
        else
        PanelDark.SetActive(false);
        
        Debug.Log("PanelDark désactivé");
        //Monster appear
        Monster.SetActive(true);
        Stinger.Play();
        //vérifier s'il est joué
        if (Stinger.isPlaying)
        {
            Debug.Log("Stinger is playing");
        }
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


        // What was that dream ?
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

        yield return new WaitForSeconds(1f);


        //Look around
        Introduction.SetActive(true);
        yield return new WaitForSeconds(10f);
        TMP_Text introTextComponent = Introduction.GetComponent<TMP_Text>();
        if (introTextComponent != null)
        {
            yield return StartCoroutine(FadeOutText(introTextComponent, 2f));
        }
        yield return new WaitForSeconds(2f);
        Introduction.SetActive(false);
        // Baseball voice
        BaseballVoice.Play();
        yield return new WaitForSeconds(BaseballVoice.clip.length + 1f);

        SpawnObjectNearCamera();
        yield return new WaitForSeconds(2f);
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

        Vector3[] directions = new Vector3[]
        {
            cameraRig.forward,
            -cameraRig.forward,
            cameraRig.right,
            -cameraRig.right
        };

        LayerMask environmentMask = 1 << 6;// Layer 6 = Environnement
        float maxRaycastDistance = 5f;
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
                    Ambiant.volume = 0f;
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