using UnityEngine;

public class PlaySoundOnCollision : MonoBehaviour
{
    public AudioSource collisionAudioSource; // Assign this in the Inspector

    private void OnCollisionEnter(Collision collision)
    {
        // Get the name of the other object, lowercased for case-insensitive comparison
        string otherName = collision.gameObject.name.ToLower();

        if ((otherName.Contains("wall") || otherName.Contains("floor")) || otherName.Contains("table") && collisionAudioSource != null)
        {
            collisionAudioSource.Play();
        }
    }
}
