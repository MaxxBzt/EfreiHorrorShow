using System.Collections;
using UnityEngine;

public class CinematiqueSequence : MonoBehaviour
{
    public GameObject blinkEffect;  
    
    void Start()
    {
        blinkEffect.SetActive(false);

        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        blinkEffect.SetActive(true);
        yield return new WaitForSeconds(2f);
        blinkEffect.SetActive(false);

    }
}
