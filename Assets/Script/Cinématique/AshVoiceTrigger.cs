using UnityEngine;

public class AshVoiceTrigger : MonoBehaviour
{
    public AudioSource voice;
    private bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("MainCamera"))
        {
            Debug.Log("AshVoiceTrigger activated: " + voice.clip.name);
            voice.Play();
            triggered = true;
            Destroy(gameObject); 
        }
    }
}
