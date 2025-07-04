using UnityEngine;
using Oculus.Interaction;
using System.Collections;

public class OneGrabBook : MonoBehaviour
{
    public AudioSource grabAudioSource; 
    private Grabbable grabbable;
    public Animator coverAnimation;
    public AudioSource CloseBookAudioSource;
    public AudioSource Ashvoice1;
    public AudioSource Ashvoice2;
    private bool hasPlayedOnce = false;


    public OVRInput.Controller controller = OVRInput.Controller.RTouch; // Ajuste selon ton besoin

    public GameObject handGrabObject; // <-- Référence de l'objet à supprimer

    void Awake()
    {
        grabbable = GetComponent<Grabbable>();
        if (grabAudioSource == null)
            grabAudioSource = GetComponent<AudioSource>();
        if (coverAnimation == null)
            coverAnimation = GetComponent<Animator>();
        if (CloseBookAudioSource == null)
            CloseBookAudioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        if (grabbable != null)
            grabbable.WhenPointerEventRaised += OnPointerEvent;
    }

    void OnDisable()
    {
        if (grabbable != null)
            grabbable.WhenPointerEventRaised -= OnPointerEvent;
    }

    private Coroutine voiceCoroutine;

    private void OnPointerEvent(PointerEvent evt)
{
    if (evt.Type == PointerEventType.Select)
    {
        // Toujours ouvrir le livre visuellement !
        if (coverAnimation != null)
            coverAnimation.SetTrigger("OpenBook");

        // Les sons + coroutine, uniquement la première fois
        if (!hasPlayedOnce)
        {
            if (grabAudioSource != null)
                grabAudioSource.Play();

            if (voiceCoroutine != null) StopCoroutine(voiceCoroutine);
            voiceCoroutine = StartCoroutine(VoiceAndHapticSequence());

            hasPlayedOnce = true; // Bloque la séquence
        }
    }
    else if (evt.Type == PointerEventType.Unselect)
    {
        if (coverAnimation != null)
            coverAnimation.SetTrigger("CloseBook");
        if (CloseBookAudioSource != null)
            CloseBookAudioSource.Play();

        if (voiceCoroutine != null) StopCoroutine(voiceCoroutine);
        OVRInput.SetControllerVibration(0, 0, controller);
    }
}


    private IEnumerator VoiceAndHapticSequence()
    {
        // Vibration légère pendant Ashvoice1
        if (Ashvoice1 != null)
        {
            Ashvoice1.Play();
            OVRInput.SetControllerVibration(1f, 0.2f, controller);
            while (Ashvoice1.isPlaying)
            {
                yield return null; // attend la frame suivante
            }
            OVRInput.SetControllerVibration(0, 0, controller); // Stop la vibration à la fin
        }

        // Pause SANS vibration (ex : 2 secondes)
        yield return new WaitForSeconds(2f);

        // Vibration forte pendant Ashvoice2
        if (Ashvoice2 != null)
        {
            Ashvoice2.Play();
            OVRInput.SetControllerVibration(1f, 1f, controller);
            while (Ashvoice2.isPlaying)
            {
                yield return null;
            }
            OVRInput.SetControllerVibration(0, 0, controller); // Stop la vibration à la fin
        }

        // Supprime l'objet HandGrab
        if (handGrabObject != null)
            Destroy(handGrabObject);
        
        Ashvoice1.Stop();
        Ashvoice2.Stop();
    }

}
