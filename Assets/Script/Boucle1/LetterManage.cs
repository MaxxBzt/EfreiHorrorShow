using UnityEngine;
using Oculus.Interaction;
using System.Collections;
using UnityEngine.UI;

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

    public bool dongplayed = false;
    public AudioSource Redreading;
    public GameObject Handgrab; 
    public AudioSource Remembervoice;

    public AudioSource DongSound;
    public GameObject fogPrefab; 
    public AudioSource WhatsHappening;
    public AudioSource earRinging;
    public AudioSource InstructionsVoice;
    public bool playAmbiant;

    public bool monsterAppear;


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

    }

    void Start()
    {
        playAmbiant = false;

        monsterAppear = false;

        SpawnKeys.prefab = Keys;
        SpawnRealkey.prefab = RealKeys;
        fogPrefab.SetActive(false); 
    }

    void Update()
    {
        if (grabbable == null) return;

        // Start grab: only trigger once per grab
        if (!isGrabbed && grabbable.SelectingPointsCount > 0)
        {
            isGrabbed = true;
            Redreading.Play();
            StartCoroutine(ReadingLetter(Redreading.clip.length+5f));
        }

        // Release grab: reset
        if (isGrabbed && grabbable.SelectingPointsCount == 0)
        {
            isGrabbed = false;
        }

        //Apparition rapide du monstre et flippant un peup
        //cherche sur la scène un objet qui s'appelle darkness




    }

    IEnumerator MonstreApparition(GameObject darkness)
    {
        playAmbiant = true;
        monsterAppear = true;
        Transform panelTransform = darkness.transform.GetChild(0);
        GameObject panel = panelTransform.gameObject;

        Image img = panel.GetComponent<Image>();
        if (img != null)
        {
            Color c = img.color;
            c.a = 1f; // Opaque
            img.color = c;
        }


        yield return new WaitForSeconds(1f);

        panel.SetActive(true);
        monsterAppear = false;

        yield return new WaitForSeconds(1f);

        panel.SetActive(false);
        monsterAppear = true;
        
        yield return new WaitForSeconds(2f);

        panel.SetActive(true);
        monsterAppear = false;

        yield return new WaitForSeconds(1f);

        panel.SetActive(false);
        monsterAppear = true;

        yield return new WaitForSeconds(2f);

        panel.SetActive(true);
        monsterAppear = false;

        yield return new WaitForSeconds(2f);

        panel.SetActive(false);
        monsterAppear = false;
        playAmbiant = false;
        yield return new WaitForSeconds(2f);

        // Play the "What's happening?" voice
        if (WhatsHappening != null)
        {
            WhatsHappening.Play();
            yield return new WaitForSeconds(WhatsHappening.clip.length + 1f);
        }

        panel.SetActive(true);
        InstructionsVoice.Play();
        yield return new WaitForSeconds(InstructionsVoice.clip.length + 1f);

        earRinging.Play();
        StartCoroutine(PanelFlashToWhite(panel, 0.5f));

        yield return new WaitForSeconds(1f);

        // Fade out the panel
        float elapsed = 0f;
        Color startColor = img.color;
        Color endColor = startColor;
        endColor.a = 0f;

        while (elapsed < 2f)
        {
            img.color = Color.Lerp(startColor, endColor, elapsed / 2f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        img.color = endColor;
        img.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);

        DongSound.Play();


        dongplayed = true;



        



    }


    // Appelle cette coroutine pour flasher le panel !
    IEnumerator PanelFlashToWhite(GameObject panel, float flashDuration = 0.3f)
    {
        Image img = panel.GetComponent<Image>();
        if (img == null) yield break;

        Color startColor = Color.black;
        Color endColor = Color.white;
        float t = 0f;

        img.color = startColor; // Commence noir

        while (t < flashDuration)
        {
            t += Time.deltaTime;
            img.color = Color.Lerp(startColor, endColor, t / flashDuration);
            yield return null;
        }
        img.color = endColor; // Assure bien blanc à la fin
    }


    IEnumerator ReadingLetter(float timewait)
    {
        // Play the reading voice
        yield return new WaitForSeconds(timewait);

            //yield return new WaitForSeconds(Redreading.clip.length);
            StartCoroutine(FadeOutAndDestroyHandgrab(5f));


        // Play the memory voice, if present
        if (Remembervoice != null)
        {
            Remembervoice.Play();
            yield return new WaitForSeconds(Remembervoice.clip.length+1f);
        }
        else
        {
            yield return null;
        }


        GameObject darkness = GameObject.Find("darkness");
        StartCoroutine(MonstreApparition(darkness));
    }

    IEnumerator FadeOutAndDestroyHandgrab(float duration)
    {
       //Chercher l'enfant comprenant dans le nom "single"
       Transform letterchild = transform.Find("Red's letter single");
       Renderer render = letterchild.GetComponent<Renderer>();
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

    void OnCollisionEnter(Collision collision)
    {
        if(collisionCount <1){
            collisionAudioSource.Play();
            collisionCount++;
        }
    }
}


