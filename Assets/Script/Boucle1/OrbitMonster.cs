using UnityEngine;

public class OrbitAndLookAtCamera : MonoBehaviour
{
    public Transform cameraTransform;
    private float orbitRadius = 8f;
    private float orbitSpeed = 140f;

    private LetterManage letterManage;


    private float currentAngle = 0f;
    public AudioSource murmur;
    private bool hasPlayed = false; // Pour ne pas relancer le son à chaque frame
    private Renderer rend;
    private GameObject letterManageObject;

    void Start() {
        rend = GetComponent<Renderer>();
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
        rend.enabled = false; // Désactiver le rendu au départ
    }

void Update()
{
    // Toujours tourner, même si la lettre n'existe pas encore !
    if (cameraTransform == null)
        return;

    currentAngle += orbitSpeed * Time.deltaTime;
    if (currentAngle > 360f)
        currentAngle -= 360f;

    Vector3 offset = new Vector3(
        Mathf.Cos(currentAngle * Mathf.Deg2Rad) * orbitRadius,
        0f,
        Mathf.Sin(currentAngle * Mathf.Deg2Rad) * orbitRadius
    );

    transform.position = cameraTransform.position + offset;
    transform.LookAt(cameraTransform.position);

    Debug.Log("lettre"+letterManage);

    // Gérer la lettre ensuite...
    if (letterManage == null)
    {
        letterManageObject = GameObject.Find("Letter(Clone)");
        if (letterManageObject != null)
        {
            letterManage = letterManageObject.GetComponent<LetterManage>();
        }
        else
        {
            rend.enabled = false;
            return;
        }
    }

    if (letterManage.monsterAppear)
    {
        rend.enabled = true;
        if (!hasPlayed)
        {
            murmur.Play();
            hasPlayed = true;
        }
    }
    else
    {
        rend.enabled = false;
        hasPlayed = false;
    }
}


}
