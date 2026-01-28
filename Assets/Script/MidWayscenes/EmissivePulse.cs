using UnityEngine;

public class EmissivePulse : MonoBehaviour
{
    public Material emissiveMaterial;
    public Color baseColor = Color.white;
    public float pulseSpeed = 2f;
    public float minIntensity = 0.5f;
    public float maxIntensity = 5f;

    void Update()
    {
        float emission = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PingPong(Time.time * pulseSpeed, 1f));
        Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);
        emissiveMaterial.SetColor("_EmissionColor", finalColor);
    }
}
