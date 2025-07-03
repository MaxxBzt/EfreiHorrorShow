using UnityEngine;

public class OrbitAndLookAtCamera : MonoBehaviour
{
    public Transform cameraTransform;
    private float orbitRadius = 8f;
    private float orbitSpeed = 140f;

    private float currentAngle = 0f;
    public AudioSource murmur;
    private bool hasPlayed = false; // Pour ne pas relancer le son à chaque frame

    void Update()
    {
        if (cameraTransform == null)
            return;

        // Joue le son une seule fois
        if (!hasPlayed)
        {
            murmur.Play();
            hasPlayed = true;
        }

        // Fait orbiter l'objet autour de la caméra
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
    }
}
