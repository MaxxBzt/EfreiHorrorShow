using UnityEngine;
using Oculus.Interaction;
using System.Collections;

public class LetterManage : MonoBehaviour
{
    public Grabbable grabbable;
    public AudioSource collisionAudioSource;
    private int collisionCount = 0;
    private bool hasCollided = false;
    private float collisionTime = 0f;
    private float grabStartTime = 0f;
    private bool isGrabbed = false;

    public GameObject Keys;
    public GameObject RealKeys;
    public bool keysCalled = false;
    public AudioClip PencilClip;

    public AudioClip Dongclip;
    private bool dongplayed = false;

    
    public AudioSource DongSound;

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
        if (DongSound == null)
        {
            DongSound = gameObject.AddComponent<AudioSource>();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnKeys.prefab = Keys;
        SpawnRealkey.prefab = RealKeys;
        DongSound.clip = Dongclip;
        DongSound.playOnAwake = false;

    }

    // Update is called once per frame
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
    // ... Dans Update()

    if (isGrabbed && grabbable.SelectingPointsCount == 0)
    {
        isGrabbed = false;

        float heldDuration = Time.time - grabStartTime;
        float timeSinceCollision = Time.time - collisionTime;
        Debug.Log("Held Duration: " + heldDuration);

        // Ajoute la condition !keysCalled
        if (!keysCalled && collisionCount >= 1 && heldDuration >= 2f)
        {
            keysCalled = true;
            StartCoroutine(SpawnKeysCoroutine(100, 2f, 5f));
        }
    }

    }

     IEnumerator SpawnKeysCoroutine(int count, float radius = 1f, float minHeight = 5f, float maxHeight = 7f)
    {
        Transform cameraT = Camera.main != null ? Camera.main.transform : null;
        Vector3 center = cameraT != null ? cameraT.position : transform.position; // fallback si pas de caméra
        yield return new WaitForSeconds(1.0f); // Attendre un peu avant de commencer le spawn
        for (int i = 0; i < count; i++)
        {
            SpawnKeys.SpawnAbovePlayer(center, 1, radius, minHeight, maxHeight);
            if (i == 15)
            {
                // Après 15 clés, spawn la vraie clé
                yield return SpawnRealKeyCoroutine();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator SpawnRealKeyCoroutine()
    {
        Transform cameraT = Camera.main != null ? Camera.main.transform : null;
        Vector3 center = cameraT != null ? cameraT.position : transform.position; // fallback si pas de caméra

        SpawnRealkey.Spawn(center, 2f, 5f);

        yield return new WaitForSeconds(0.1f);
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

                // Joue le son Dong une fois si collisionCount > 2
            if (collisionCount == 2 && !dongplayed && DongSound != null)
            {
                Debug.Log(collisionCount +" HEYTURSZERDTFYYUGUH");
                collisionAudioSource.clip = Dongclip;
            }
            else
            {
                collisionAudioSource.clip = PencilClip; // Réinitialise le clip si ce n'est pas le cas
            }
            collisionAudioSource.Play();
            hasCollided = true;
            collisionTime = Time.time;
        }
    }
}
