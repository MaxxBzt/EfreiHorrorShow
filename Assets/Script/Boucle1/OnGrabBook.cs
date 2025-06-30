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
            if (grabAudioSource != null)
                grabAudioSource.Play();
            if (coverAnimation != null)
                coverAnimation.SetTrigger("OpenBook");

            // Lance la séquence voix + haptique
            if (voiceCoroutine != null) StopCoroutine(voiceCoroutine);
            voiceCoroutine = StartCoroutine(VoiceAndHapticSequence());
        }
        else if (evt.Type == PointerEventType.Unselect)
        {
            if (coverAnimation != null)
                coverAnimation.SetTrigger("CloseBook");
            if (CloseBookAudioSource != null)
                CloseBookAudioSource.Play();

            // Stop toute séquence et vibration
            if (voiceCoroutine != null) StopCoroutine(voiceCoroutine);
            OVRInput.SetControllerVibration(0, 0, controller);
        }
    }

    private IEnumerator VoiceAndHapticSequence()
    {
        // Vibration légère pendant la première voix
        OVRInput.SetControllerVibration(1f, 0.2f, controller);

        // Lecture de la première voix
        if (Ashvoice1 != null)
            Ashvoice1.Play();

        // Attend la fin de la première voix
        if (Ashvoice1 != null)
            yield return new WaitForSeconds(Ashvoice1.clip.length);

        // Garde la vibration légère pendant 2 secondes
        yield return new WaitForSeconds(1f);

        // Vibration plus forte et lecture de la deuxième voix
        OVRInput.SetControllerVibration(1f, 1f, controller);
        if (Ashvoice2 != null)
            Ashvoice2.Play();

        // Attend la fin de la deuxième voix
        if (Ashvoice2 != null)
            yield return new WaitForSeconds(Ashvoice2.clip.length);

        // Stop la vibration
        OVRInput.SetControllerVibration(0, 0, controller);

        // Supprime l'objet HandGrab
        if (handGrabObject != null)
            Destroy(handGrabObject);
    }
}
