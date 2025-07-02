using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartScene : MonoBehaviour
{
    public CanvasGroup introCanvas;
    public TextMeshProUGUI introText;
    public string nextSceneName = "Séquence0";

    void Start()
    {
        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        // Hide everything initially
        introCanvas.alpha = 0;
        introText.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);

        // Show and fade in
        introText.text = "This will be your voice during this story. Remember it.";
        introText.gameObject.SetActive(true);
        yield return FadeCanvasAlpha(1f, 1.5f);

        // Hold 5 seconds
        yield return new WaitForSeconds(5f);

        // Fade out
        yield return FadeCanvasAlpha(0f, 1.5f);
        introText.gameObject.SetActive(false);

        // Optional short pause
        yield return new WaitForSeconds(0.5f);

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator FadeCanvasAlpha(float targetAlpha, float duration)
    {
        float startAlpha = introCanvas.alpha;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float eased = Mathf.SmoothStep(startAlpha, targetAlpha, t / duration);
            introCanvas.alpha = eased;
            yield return null;
        }

        introCanvas.alpha = targetAlpha;
    }
}

