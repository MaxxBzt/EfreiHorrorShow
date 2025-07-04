using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LightGateTrigger : MonoBehaviour
{
    public string nextSceneName = "Boucle2";

    [Header("Fade Settings")]
    public CanvasGroup instructionCanvas; 
    public float fadeDuration = 1.5f;

    private bool hasEntered = false;

    void Start()
    {
        // Lance la coroutine d'attente de 10 secondes
        StartCoroutine(AutoNextSceneAfterDelay(10f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasEntered) return;

        if (other.CompareTag("MainCamera") || other.CompareTag("Player"))
        {
            hasEntered = true;
            StartCoroutine(FadeThenLoad());
        }
    }

    IEnumerator AutoNextSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!hasEntered)
        {
            hasEntered = true;
            StartCoroutine(FadeThenLoad());
        }
    }

    IEnumerator FadeThenLoad()
    {
        if (instructionCanvas != null)
        {
            float t = 0f;
            float startAlpha = instructionCanvas.alpha;

            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                instructionCanvas.alpha = Mathf.Lerp(startAlpha, 0f, t / fadeDuration);
                yield return null;
            }
        }

        // Petite pause dramatique
        yield return new WaitForSeconds(0.5f);

        // Changement de scène
        SceneManager.LoadScene("Boucle2");
    }
}
