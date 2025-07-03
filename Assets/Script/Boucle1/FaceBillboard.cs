using UnityEngine;

public class FaceBillboard : MonoBehaviour
{
    public Camera targetCamera;

    void Start()
    {
        // Si aucune caméra n'est assignée, utilise la caméra principale
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    void LateUpdate()
    {
        if (targetCamera == null) return;

        // Oriente l'objet pour faire face à la caméra
        transform.forward = targetCamera.transform.forward;
    }
}