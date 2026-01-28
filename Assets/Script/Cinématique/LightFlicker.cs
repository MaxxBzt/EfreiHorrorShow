using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light targetLight;
    public float minIntensity = 0.8f;
    public float maxIntensity = 1.2f;
    public float flickerSpeed = 0.1f;

    private void Update()
    {
        targetLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PerlinNoise(Time.time * flickerSpeed, 0));
    }
}