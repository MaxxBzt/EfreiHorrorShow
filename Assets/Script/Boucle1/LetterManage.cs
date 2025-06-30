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

    private bool isGrabbed = false; // Properly used now!

    public GameObject Keys;
    public GameObject RealKeys;
    public bool keysCalled = false;
    public AudioClip PencilClip;

    public AudioClip Dongclip;
    public bool dongplayed = false;
    public AudioSource Redreading;
    public GameObject Handgrab; 
    public AudioSource Remembervoice;

    public AudioSource DongSound;
    public GameObject fogPrefab; 

    public GameObject monstre;

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

    void Start()
    {
        SpawnKeys.prefab = Keys;
        SpawnRealkey.prefab = RealKeys;
        DongSound.clip = Dongclip;
        DongSound.playOnAwake = false;
        fogPrefab.SetActive(false); 
    }

    void Update()
    {
        if (grabbable == null) return;

        // Start grab: only trigger once per grab
        if (!isGrabbed && grabbable.SelectingPointsCount > 0)
        {
            isGrabbed = true;
            StartCoroutine(ReadingLetter());
        }

        // Release grab: reset
        if (isGrabbed && grabbable.SelectingPointsCount == 0)
        {
            isGrabbed = false;
        }

       monstre.SetActive(true);
       monstre.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 10f;

    }

    IEnumerator ReadingLetter()
    {
        // Play the reading voice
        if (Redreading != null)
        {
            Redreading.Play();
            // Fade out and destroy Handgrab after reading is finished (+1s buffer)
            StartCoroutine(FadeOutAndDestroyHandgrab(Redreading.clip.length + 1f));
        }

        // Play the memory voice, if present
        if (Remembervoice != null)
        {
            Remembervoice.Play();
            yield return new WaitForSeconds(Remembervoice.clip.length);
        }
        else
        {
            yield return null;
        }
    }

    IEnumerator FadeOutAndDestroyHandgrab(float duration)
    {
       Renderer render = gameObject.GetComponentInChildren<Renderer>();
       fogPrefab.SetActive(true);
       yield return new WaitForSeconds(4f);
       render.enabled = false;
       // decrease gently the rate over time of the fog until 0
         if (fogPrefab != null)
            {
                ParticleSystem ps = fogPrefab.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    var emission = ps.emission;
                    float startRate = emission.rateOverTime.constant;
                    float t = 0f;
                    while (t < duration)
                    {
                        float newRate = Mathf.Lerp(startRate, 0f, t / duration);
                        emission.rateOverTime = newRate;
                        t += Time.deltaTime;
                        yield return null;
                    }
                    emission.rateOverTime = 0f; // Assure que c'est à 0 à la fin
                }
            }

      Handgrab.SetActive(false);
    }

    IEnumerator SpawnKeysCoroutine(int count, float radius = 1f, float minHeight = 5f, float maxHeight = 7f)
    {
        Transform cameraT = Camera.main != null ? Camera.main.transform : null;
        Vector3 center = cameraT != null ? cameraT.position : transform.position;
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < count; i++)
        {
            SpawnKeys.SpawnAbovePlayer(center, 1, radius, minHeight, maxHeight);
            if (i == 15)
            {
                // After 15 keys, spawn the real key
                yield return SpawnRealKeyCoroutine();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator SpawnRealKeyCoroutine()
    {
        Transform cameraT = Camera.main != null ? Camera.main.transform : null;
        Vector3 center = cameraT != null ? cameraT.position : transform.position;
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

            // Play Dong only once if collisionCount == 2
            if (collisionCount == 2 && !dongplayed && DongSound != null)
            {
                Debug.Log(collisionCount + " HEYTURSZERDTFYYUGUH");
                collisionAudioSource.clip = Dongclip;
                dongplayed = true;
            }
            else
            {
                collisionAudioSource.clip = PencilClip;
            }
            collisionAudioSource.Play();
            hasCollided = true;
            collisionTime = Time.time;
        }
    }
}
