using System.Collections;
using UnityEngine;

public class IntroMoment : MonoBehaviour
{
    public string backgroundSoundName = "MenuAmbience"; // nom dans AudioManager

    void Start()
    {
        // Lancer directement le son d’ambiance dès que la scène se charge
        AudioManager.instance.Play(backgroundSoundName);
    }
}
