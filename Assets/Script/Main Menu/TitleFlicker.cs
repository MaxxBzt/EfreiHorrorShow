using TMPro;
using UnityEngine;

public class TextFlicker : MonoBehaviour
{
    public TextMeshPro text; // CHANGED from TextMeshProUGUI
    public float flickerInterval = 5f;
    public float flickerDuration = 0.2f;

    void Start()
    {
        if (!text) text = GetComponent<TextMeshPro>();
        InvokeRepeating(nameof(FlickerOnce), flickerInterval, flickerInterval);
    }

    void FlickerOnce()
    {
        StartCoroutine(Flicker());
    }

    System.Collections.IEnumerator Flicker()
    {
        text.enabled = false;
        yield return new WaitForSeconds(flickerDuration);
        text.enabled = true;
    }
}
