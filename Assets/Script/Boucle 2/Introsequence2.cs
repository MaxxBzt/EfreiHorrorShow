using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI; // Assurez-vous d'avoir cette ligne si vous utilisez des UI
using Unity.VisualScripting; // Nécessaire pour TMP_Text
using UnityEngine.SceneManagement; // Nécessaire pour la gestion des scènes


public class Introsequence2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject blinkEffect;
    public GameObject introText;
    public GameObject Monster;
    public GameObject PanelDark;

    public AudioSource VoiceBeginning;
    public AudioSource stinger;

    void Start()
    {
        blinkEffect.SetActive(false);
        introText.SetActive(false);
        Monster.SetActive(false);
        PanelDark.SetActive(true);

        StartCoroutine(PlayIntro());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PlayIntro()
    {
        PanelDark.SetActive(true);
        yield return new WaitForSeconds(5f);
        PanelDark.SetActive(false);
        yield return new WaitForSeconds(1f);
        stinger.Play();
        Monster.SetActive(true);
        yield return new WaitForSeconds(stinger.clip.length);
        Monster.SetActive(false);
        blinkEffect.SetActive(true);

        Material mat = blinkEffect.GetComponent<MeshRenderer>().material;

        float duration = 6f;
        float t = 0f;

        while (t < duration)
        {
            mat.SetFloat("_time", t + 1);
            t += Time.deltaTime;
            yield return null;
            mat.SetFloat("_width", Mathf.Lerp(.5f, .8f, t / duration));
        }

        blinkEffect.SetActive(false);

        blinkEffect.SetActive(false);

        introText.SetActive(true);
        VoiceBeginning.Play();
        yield return new WaitForSeconds(VoiceBeginning.clip.length);

        TMP_Text tmp = introText.GetComponent<TMP_Text>();
        if (tmp != null)
        {
            yield return StartCoroutine(FadeOutText(tmp, 2f));
        }
        yield return new WaitForSeconds(5f);
        introText.SetActive(false);

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("EndGame"); // Assurez-vous que le numéro de scène est correct
    }

        IEnumerator FadeOutText(TMP_Text textElement, float duration)
    {
        float elapsedTime = 0f;
        Color originalColor = textElement.color;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            textElement.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        textElement.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        
    }

        
}
