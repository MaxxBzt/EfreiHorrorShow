using UnityEngine;

public class TeleportRig : MonoBehaviour
{
    public Transform targetPosition;

    public void Teleport()
    {
        if (targetPosition != null)
        {
            transform.position = targetPosition.position;
            transform.rotation = targetPosition.rotation;
        }
    }
}
