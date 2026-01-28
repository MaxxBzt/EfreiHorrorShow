using UnityEngine;

public class TeleportRig : MonoBehaviour
{
    public void TeleportTo(Transform targetPosition)
    {
        if (targetPosition != null)
        {
            transform.position = targetPosition.position;
            transform.rotation = targetPosition.rotation;
        }
    }
}
