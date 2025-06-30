using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class IntroSequenceManager : MonoBehaviour
{
    public CanvasGroup introCanvas;
    public TextMeshProUGUI introText;

    public AudioSource voiceSource;
    public AudioClip ashLine1;
    public AudioClip clockLoop;
    public AudioClip whisperClip;
    public AudioClip heartbeatClip;

    public GameObject redEyesFlicker; // your glowing red eyes PNG (disabled by default)
    public Light flashLight; // optional, for flash at final CLACK
    public float monsterGrowDuration = 2f;

    public GameObject monsterObject;
    public string nextSceneName = "Main Menu";


    void Start()
    {
        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        // Afficher texte d’intro
        introCanvas.alpha = 0;
        introText.text = "Efrei Horror Show\npresents";
        yield return FadeCanvas(1, 1.5f);
        yield return new WaitForSeconds(5f);
        yield return FadeCanvas(0, 1.5f); // fondu vers noir

        // 2. Voix-off Ash
        yield return new WaitForSeconds(1f);
        voiceSource.clip = ashLine1;
        voiceSource.Play();
        yield return new WaitForSeconds(ashLine1.length + 0.5f);

        // 0–9s Clock ticking in background
        voiceSource.clip = clockLoop;
        voiceSource.loop = false;
        voiceSource.Play();

        // 3s: Eyes flicker + whisper
        yield return new WaitForSeconds(3f);
        redEyesFlicker.SetActive(true);
        voiceSource.PlayOneShot(whisperClip);

        yield return new WaitForSeconds(1.5f);
        redEyesFlicker.SetActive(false);

        // 6s: Eyes flash again + heartbeat
        yield return new WaitForSeconds(1.5f); // (now at 6s mark)
        redEyesFlicker.SetActive(true);
        voiceSource.PlayOneShot(heartbeatClip); // heartbeat sound

        yield return new WaitForSeconds(1.5f);
        redEyesFlicker.SetActive(false);


        monsterObject.SetActive(true);
        StartCoroutine(GrowMonster());

        yield return StartCoroutine(FadeToBlack());
        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator GrowMonster()
    {
        Vector3 originalScale = monsterObject.transform.localScale;
        Vector3 targetScale = originalScale * 5f;

        float t = 0f;
        while (t < monsterGrowDuration)
        {
            t += Time.deltaTime;
            float progress = t / monsterGrowDuration;
            monsterObject.transform.localScale = Vector3.Lerp(originalScale, targetScale, progress);
            yield return null;
        }
    }

    IEnumerator FadeToBlack()
    {
        float duration = 1.5f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            introCanvas.alpha = Mathf.Lerp(0, 1, t / duration);
            yield return null;
        }

        introCanvas.alpha = 1;
    }

    IEnumerator FadeCanvas(float targetAlpha, float duration)
    {
        float startAlpha = introCanvas.alpha;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            introCanvas.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }

        introCanvas.alpha = targetAlpha;
    }


}
