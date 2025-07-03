using UnityEngine;
using UnityEngine.UI; // Assurez-vous d'avoir cette ligne si vous utilisez des UI
using System.Collections;
using Oculus.Interaction; // Assurez-vous d'avoir cette ligne si vous utilisez Grabbable


public class FindingKey : MonoBehaviour
{
    private LetterManage letterManage; 
    public AudioClip Soundeffect;
    public AudioSource SoundSource;

    public AudioClip AshSearchingClip;
    public AudioSource AshSearching;

    public AudioClip MonsterEndingClip;
    public AudioSource MonsterEnding;
    public AudioSource Stinger;

    public GameObject monster;
    public GameObject black;

    public AudioSource RealKeySoundvoice;
    public bool specialEndingLaunched = false;



    private float time = 0f;
    public bool timeStarted = false;
    private bool endingPanelLaunched = false;
    private bool ashplayed = false;


    void Start()
    {

        AshSearching.clip = AshSearchingClip;
        MonsterEnding.clip = MonsterEndingClip;
        

        if (SoundSource == null)
        {
            SoundSource = gameObject.AddComponent<AudioSource>();
        }
        SoundSource.clip = Soundeffect;
        GetComponent<SpawningMonster>().enabled = false;


        // Désactiver les GameObjects au début
        monster.SetActive(false);
        black.SetActive(false);
        timeStarted = false;
    }

    void Update()
    {
        GameObject letterObject = GameObject.Find("Letter(Clone)");
        if (letterObject == null) return;
        letterManage = letterObject.GetComponent<LetterManage>();
        if (letterManage == null) return;


        if (letterManage.dongplayed)
        {
            if (!timeStarted)
            {
                timeStarted = true;
            }
            SoundSource.Play();
            StartCoroutine(HapticFeedbackRoutine());
            //Activer le script du Spawn du monstre
            GetComponent<SpawningMonster>().enabled = true;

            letterManage.dongplayed = false;
        }

        if(timeStarted)
        {
            time += Time.deltaTime;
        }
        if (time >= 15f && !ashplayed)
        {
            AshSearching.Play();
            ashplayed = true;
        }

        if (time >= 30f && !endingPanelLaunched)
        {
            endingPanelLaunched = true;
            // Désactiver le script du Spawn du monstre
            GetComponent<SpawningMonster>().enabled = false;
            // Arrêter la musique de recherche
            AshSearching.Stop();
            // Arrêter les vibrations
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);

            StartCoroutine(ShowEndingPanel());
            

        }


         foreach (GameObject realKey in GameObject.FindGameObjectsWithTag("realkey"))
        {
            // On récupère le Grabbable (le composant qui permet le grab)
            Grabbable grabbable = realKey.GetComponent<Grabbable>();
            if (grabbable != null && grabbable.SelectingPointsCount > 0)
            {
                // La clé est attrapée !
                specialEndingLaunched = true; // Pour ne le faire qu'une fois
                Debug.Log("REAL KEY ATTRAPÉE !");

                // → Met ici ton ending spécial, panel, sons, changement de scène, etc.
                StartCoroutine(SpecialEndingCoroutine());
                break; // On sort de la boucle (une seule fois)
            }
        }


    }

    private System.Collections.IEnumerator HapticFeedbackRoutine()
    {
        float duration = 31f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Premier battement (poum)
            OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);
            OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.LTouch);
            yield return new WaitForSeconds(0.12f);

            // Petite pause entre les deux battements
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
            yield return new WaitForSeconds(0.08f);

            // Deuxième battement (poum)
            OVRInput.SetControllerVibration(1, 0.6f, OVRInput.Controller.RTouch);
            OVRInput.SetControllerVibration(1, 0.6f, OVRInput.Controller.LTouch);
            yield return new WaitForSeconds(0.09f);

            // Pause courte avant de recommencer (pas de longue pause)
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
            yield return new WaitForSeconds(0.12f); // Ajuste cette valeur pour accélérer ou ralentir le rythme

            elapsed += 0.12f + 0.08f + 0.09f + 0.12f;
        }

        // Arrête toute vibration à la fin
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
    }

    IEnumerator ShowEndingPanel()
    {
        DisableAllMonstersAndKeys();
        black.SetActive(true);
        yield return new WaitForSeconds(4f);
        MonsterEnding.Play();
        yield return new WaitForSeconds(MonsterEnding.clip.length+2f);
        black.SetActive(false);
        Stinger.Play();
        monster.SetActive(true);
        yield return new WaitForSeconds(2f);
        monster.SetActive(false);
        black.SetActive(true);

    }

    IEnumerator SpecialEndingCoroutine()
    {
        specialEndingLaunched = true; // Pour ne le faire qu'une fois
        DisableAllMonstersAndKeys();
        black.SetActive(true);
        yield return new WaitForSeconds(4f);
        RealKeySoundvoice.Play();
        yield return new WaitForSeconds(RealKeySoundvoice.clip.length + 2f);

    }


    void DisableAllMonstersAndKeys()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            string name = obj.name.ToLower();
            if ((name.Contains("monstre") || name.Contains("monster") || name.Contains("keys") || name.Contains("key")))
            {
                obj.SetActive(false);
            }
        }
    }



}
