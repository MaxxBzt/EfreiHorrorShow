using UnityEngine;
using UnityEngine.XR;

public class HapticFeedback : MonoBehaviour
{
    public XRNode controllerNode = XRNode.RightHand; // ou LeftHand
    public float amplitude = 0.5f;  // 0.0 à 1.0
    public float duration = 0.2f;   // en secondes

    public void TriggerHaptic()
    {
        StartCoroutine(SendHapticsCoroutine());
    }

    private System.Collections.IEnumerator SendHapticsCoroutine()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);
        if (device.isValid &&
            device.TryGetHapticCapabilities(out HapticCapabilities capabilities) &&
            capabilities.supportsImpulse)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                device.SendHapticImpulse(0, amplitude, 0.05f); // envoie répétée
                elapsedTime += 0.05f;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        if (device.TryGetFeatureValue(CommonUsages.primaryButton, out bool pressed) && pressed)
        {
            TriggerHaptic();
        }
    }
}