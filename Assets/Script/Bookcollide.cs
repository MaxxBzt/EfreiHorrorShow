using UnityEngine;
using Oculus.Interaction;

public class PlaySoundOnCollision : MonoBehaviour
{
    public AudioSource collisionAudioSource; // Assign in Inspector
    public Grabbable grabbable; // Assign in Inspector or auto-find in Awake

    void Awake()
    {
        // If not assigned, find it in children
        if (grabbable == null)
            grabbable = GetComponentInChildren<Grabbable>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        string otherName = collision.gameObject.name.ToLower();

        // Not grabbed
        bool isNotGrabbed = grabbable == null || grabbable.SelectingPointsCount == 0;

        if (isNotGrabbed && (
                otherName.Contains("wall") ||
                otherName.Contains("floor") ||
                otherName.Contains("table")
            ) && collisionAudioSource != null)
        {
            collisionAudioSource.Play();
        }
    }
}
