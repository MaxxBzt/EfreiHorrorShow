using UnityEngine;

public class TeleportBack : MonoBehaviour
{
    public Transform targetPosition;

    public void TeleportAgain()
    {
        if (targetPosition != null)
        {
            transform.position = targetPosition.position;
            transform.rotation = targetPosition.rotation;
        }
    }
}
