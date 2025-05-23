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


    private bool hasCollided = false;
    private float collisionTime = 0f;
    private float grabStartTime = 0f;
    private bool isGrabbed = false;

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
        if (grabbable == null) return;

        // Start grab
        if (!isGrabbed && grabbable.SelectingPointsCount > 0)
        {
            isGrabbed = true;
            grabStartTime = Time.time;
        }
        Debug.Log(collisionCount);

        // Release grab
        if (isGrabbed && grabbable.SelectingPointsCount == 0)
        {
            isGrabbed = false;


            float heldDuration = Time.time - grabStartTime;
            float timeSinceCollision = Time.time - collisionTime;
            Debug.Log("Held Duration: " + heldDuration);

            if (collisionCount >= 1 && heldDuration >= 5f && voice != null)
            {
                StartCoroutine(PlayVoiceAndSpawn());  // fait parler la voix du père pour continuer l'histoire
            }

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
        voice.Play();

        yield return new WaitForSeconds(5f);

        Vector3 origin = transform.position;
        SpawnLetter.Spawn(origin); 
    }



}
