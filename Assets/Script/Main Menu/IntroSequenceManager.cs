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

    public GameObject redEyesFlicker; 
    public float monsterGrowDuration = 8.5f;


    public GameObject monsterObject;
    public string nextSceneName = "Main Menu";


    void Start()
    {
        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
    
        // 1. Hide text initially
        introText.gameObject.SetActive(false);
        introCanvas.alpha = 0;

        yield return new WaitForSeconds(2f);

        // 3. Show + fade
        introText.text = "Efrei Horror Show\npresents";
        introText.gameObject.SetActive(true);
        yield return FadeTextCanvas(1f, 1.5f);

        // 4. Hold
        yield return new WaitForSeconds(5f);

        // 5. Fade out
        yield return FadeTextCanvas(0f, 1.5f);
        introText.gameObject.SetActive(false);




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
        yield return new WaitForSeconds(1f);
        redEyesFlicker.SetActive(true);
        voiceSource.PlayOneShot(whisperClip);

        yield return new WaitForSeconds(1.5f);
        redEyesFlicker.SetActive(false);

        // 6s: Eyes flash again + heartbeat
        yield return new WaitForSeconds(1.5f); // (now at 6s mark)
        redEyesFlicker.SetActive(true);
        voiceSource.PlayOneShot(heartbeatClip); 

        yield return new WaitForSeconds(1.5f);
        redEyesFlicker.SetActive(false);

         yield return new WaitForSeconds(1.5f);
        redEyesFlicker.SetActive(true);

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
            float progress = Mathf.SmoothStep(0f, 1f, t / monsterGrowDuration);
            monsterObject.transform.localScale = Vector3.Lerp(originalScale, targetScale, progress);
            yield return null;
        }
    }

    IEnumerator FadeToBlack()
    {
        float duration = 2.5f; // longer, slower fade
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float eased = Mathf.SmoothStep(0f, 1f, t / duration);
            introCanvas.alpha = eased;
            yield return null;
        }

        introCanvas.alpha = 1;

        // Load scene only once fade fully finished
        SceneManager.LoadScene(nextSceneName);
    }



    IEnumerator FadeTextCanvas(float targetAlpha, float duration)
    {
        float startAlpha = introCanvas.alpha;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float eased = Mathf.SmoothStep(startAlpha, targetAlpha, t); // smooth animation
            introCanvas.alpha = eased;
            yield return null;
        }

        introCanvas.alpha = targetAlpha;
    }




}
