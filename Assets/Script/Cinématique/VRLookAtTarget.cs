using UnityEngine;

public class XRLookAtTarget : MonoBehaviour
{
    public Transform xrOrigin; // XR Origin (la racine du rig)
    public Transform head;     // Main Camera (la tête du joueur)
    public Transform target;   // Le point à regarder
    public float rotationSpeed = 2f;

    private bool shouldRotate = false;

    void Update()
    {
        if (!shouldRotate) return;

        // Calcule la direction horizontale vers la cible
        Vector3 direction = target.position - head.position;
        direction.y = 0f;

        // Rotation à atteindre
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion headYRotation = Quaternion.Euler(0, head.eulerAngles.y, 0);
        Quaternion deltaRotation = targetRotation * Quaternion.Inverse(headYRotation);

        // Applique la rotation au XR Origin
        Quaternion newRotation = deltaRotation * xrOrigin.rotation;
        xrOrigin.rotation = Quaternion.Slerp(xrOrigin.rotation, newRotation, Time.deltaTime * rotationSpeed);
    }

    public void TriggerLookAt()
    {
        shouldRotate = true;
    }
}