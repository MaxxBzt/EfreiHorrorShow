using UnityEngine;
using Oculus.Interaction;

public class OneGrabBook : MonoBehaviour
{
    public AudioSource grabAudioSource; 
    private Grabbable grabbable;
    public Animator coverAnimation;
    public AudioSource CloseBookAudioSource;

    void Awake()
    {
        grabbable = GetComponent<Grabbable>();
        if (grabAudioSource == null)
            grabAudioSource = GetComponent<AudioSource>();
        if (coverAnimation == null)
            coverAnimation = GetComponent<Animator>();
            if (CloseBookAudioSource == null)
            CloseBookAudioSource = GetComponent<AudioSource>();}

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

    //Fonction pour ouvrir et fermer le livre
    private void OnPointerEvent(PointerEvent evt)
    {
        if (evt.Type == PointerEventType.Select)
        {
            if (grabAudioSource != null)
                grabAudioSource.Play();
            if (coverAnimation != null)
                coverAnimation.SetTrigger("OpenBook");
        }
        else if (evt.Type == PointerEventType.Unselect)
        {
            if (coverAnimation != null)
                coverAnimation.SetTrigger("CloseBook");
            if (CloseBookAudioSource != null)
                CloseBookAudioSource.Play();
        }
    }
}
