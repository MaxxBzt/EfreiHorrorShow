using System.Collections;
using UnityEngine;

public class IntroSequenceVR : MonoBehaviour
{
    public GameObject blinkEffect;  
    public GameObject introText; 

    void Start()
    {
        blinkEffect.SetActive(false);
        introText.SetActive(false);

        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        blinkEffect.SetActive(true);
        yield return new WaitForSeconds(4f);
        blinkEffect.SetActive(false);

        introText.SetActive(true);
        yield return new WaitForSeconds(10f); 
        introText.SetActive(false);
    }
}