using UnityEngine;

public class ConditionalVoicePlayback : MonoBehaviour
{
    public AshVoiceID voiceID;

    void OnEnable()
    {
        if (!FindObjectOfType<AshVoiceManager>().ShouldPlayVoiceInTimeline(voiceID))
        {
            GetComponent<AudioSource>().Stop(); // bloque le son
        }
    }
}
