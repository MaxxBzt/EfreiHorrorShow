using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndScreen : MonoBehaviour
{
    public CanvasGroup outroCanvas;
    public TextMeshProUGUI outroText;
    public float fadeDuration = 1f;
    public float blockDelay = 2.5f;
    public string nextScene = "Main Menu";

    public AudioSource audioSource;
    public AudioClip dramaticClip;
    public AudioClip monsterLaughClip;

    void Start()
    {
        StartCoroutine(OutroSequence());
    }

    IEnumerator OutroSequence()
    {
        outroCanvas.alpha = 0;
        outroText.text = "";
        outroText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        yield return FadeCanvasAlpha(1f, fadeDuration);

        // Block 1
        outroText.text = "Thank you for playing this prototype of\nASHES GUILT.";
        yield return new WaitForSeconds(blockDelay);

        // Fade out & next block
        yield return FadeCanvasAlpha(0f, fadeDuration);
        outroText.text = "This is only the beginning...";
        yield return FadeCanvasAlpha(1f, fadeDuration);
        yield return new WaitForSeconds(blockDelay);

        // Final block
        yield return FadeCanvasAlpha(0f, fadeDuration);
        outroText.text = "To continue Ash’s story,\nsupport the full release and descend deeper into the guilt.";
        yield return FadeCanvasAlpha(1f, fadeDuration);

        // 🎧 Play the first clip
        if (audioSource != null && dramaticClip != null)
        {
            audioSource.clip = dramaticClip;
            audioSource.Play();

            // Wait for the first clip to finish
            yield return new WaitForSeconds(dramaticClip.length + 0.2f); // optional short pause

            // 🎧 Play the second clip (e.g., laugh)
            if (monsterLaughClip != null)
            {
                audioSource.PlayOneShot(monsterLaughClip, 0.5f); 
            }
        }


        yield return new WaitForSeconds(4f);

        // Fade out to end
        yield return FadeCanvasAlpha(0f, fadeDuration);
        SceneManager.LoadScene(nextScene);
    }

    IEnumerator FadeCanvasAlpha(float targetAlpha, float duration)
    {
        float startAlpha = outroCanvas.alpha;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            outroCanvas.alpha = Mathf.SmoothStep(startAlpha, targetAlpha, t / duration);
            yield return null;
        }

        outroCanvas.alpha = targetAlpha;
    }
}