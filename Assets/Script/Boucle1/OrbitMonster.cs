using UnityEngine;

public class OrbitAndLookAtCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public float spawnDistance = 20f; // Distance devant la caméra
    public float moveAmplitude = 2f; // Amplitude du mouvement gauche-droite
    public float moveSpeed = 1f;     // Vitesse du mouvement gauche-droite

    private LetterManage letterManage;

    private float timeElapsed = 0f;
    public AudioSource murmur;
    private bool hasPlayed = false;

    private Renderer faceRenderer;
    private GameObject letterManageObject;

    // Pour gérer les particules :
    public ParticleSystem[] particleSystems;

    void Start()
    {
        // On va chercher le renderer sur "Face/Plane"
        Transform facePlane = transform.Find("Face/Plane");
        if (facePlane != null)
            faceRenderer = facePlane.GetComponent<Renderer>();
        else
            Debug.LogWarning("Face/Plane introuvable dans la hiérarchie du monstre");

        // Particules
        if (particleSystems == null || particleSystems.Length == 0)
            particleSystems = GetComponentsInChildren<ParticleSystem>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        if (faceRenderer != null)
            faceRenderer.enabled = false;
        SetParticlesActive(false);
    }

    void Update()
    {
        if (cameraTransform == null)
            return;

        timeElapsed += Time.deltaTime;

        // Calcul de la position devant la caméra, plus oscillation en X
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        Vector3 spawnPoint = cameraTransform.position + forward * spawnDistance;

        // Mouvement sinusoïdal gauche-droite
        float xOffset = Mathf.Sin(timeElapsed * moveSpeed) * moveAmplitude;
        Vector3 offset = right * xOffset;

        // Nouvelle position
        transform.position = spawnPoint + offset;

        // Toujours regarder la caméra
        transform.LookAt(cameraTransform.position, Vector3.up);

        // Gestion LetterManage...
        if (letterManage == null)
        {
            letterManageObject = GameObject.Find("Letter(Clone)");
            if (letterManageObject != null)
                letterManage = letterManageObject.GetComponent<LetterManage>();
            else
            {
                if (faceRenderer != null) faceRenderer.enabled = false;
                SetParticlesActive(false);
                return;
            }
        }

        if (letterManage.monsterAppear)
        {
            if (faceRenderer != null) faceRenderer.enabled = true;
            SetParticlesActive(true);
            if (!hasPlayed)
            {
                murmur.Play();
                hasPlayed = true;
            }
        }
        else
        {
            if (faceRenderer != null) faceRenderer.enabled = false;
            SetParticlesActive(false);
            hasPlayed = false;
        }
        if (!letterManage.playAmbiant)
        {
            murmur.Stop();
        }
    }

    private void SetParticlesActive(bool state)
    {
        if (particleSystems != null)
        {
            foreach (var ps in particleSystems)
            {
                var emission = ps.emission;
                emission.enabled = state;
            }
        }
    }
}
