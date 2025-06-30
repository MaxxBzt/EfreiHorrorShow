using UnityEngine;
using Oculus.Interaction;
using System.Collections;


public class PlaySoundOnCollision : MonoBehaviour
{
    public AudioSource collisionAudioSource;
    public AudioSource voice;               // Resolved at runtime
    public Grabbable grabbable;
    private int collisionCount = 0;

    public GameObject letterPrefab;
    public AudioSource whoTalked;
    public AudioSource AshePanickedVoice;


    private bool hasCollided = false;
    private float collisionTime = 0f;
    private float grabStartTime = 0f;
    private bool isGrabbed = false;
    private bool voiceplayed = false;

    void Start()
    {
        SpawnLetter.prefab = letterPrefab;
    }

    void Awake()
    {
        if (grabbable == null)
        {
            grabbable = GetComponentInChildren<Grabbable>();
        }
        if (collisionAudioSource == null)
        {
            collisionAudioSource = GetComponent<AudioSource>();
        }
        if (voice == null)
        {
            Debug.Log("No voice audio source assigned. Attempting to find one.");
            voice = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (collisionCount > 1 && voiceplayed == false)
        {
            voiceplayed = true; // On bloque tout de suite
            StartCoroutine(PlayVoiceAndSpawn());
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        string otherName = collision.gameObject.name.ToLower();

        bool isNotGrabbed = grabbable == null || grabbable.SelectingPointsCount == 0;

        if (isNotGrabbed && (
            otherName.Contains("wall") ||
            otherName.Contains("floor") ||
            otherName.Contains("table")
        ) && collisionAudioSource != null)
        {
            collisionCount++;
            collisionAudioSource.Play();
            hasCollided = true;
            collisionTime = Time.time;
        }
    }

    IEnumerator PlayVoiceAndSpawn()
    {
        yield return new WaitForSeconds(1f);
        whoTalked.Play();
        yield return new WaitForSeconds(5f);
        voice.Play();

        yield return new WaitForSeconds(voice.clip.length);
        if (AshePanickedVoice != null)
        {
            AshePanickedVoice.Play();
        }
        yield return new WaitForSeconds(AshePanickedVoice.clip.length + 1f);

        // Vector3 origin = transform.position;  // <- inutile ici
        SpawnLetter.Spawn(Camera.main.transform.position, 0.5f); // 0.5m autour de la caméra
    }




}
