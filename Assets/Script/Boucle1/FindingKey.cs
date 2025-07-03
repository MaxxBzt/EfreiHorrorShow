using UnityEngine;
using UnityEngine.UI; // Assurez-vous d'avoir cette ligne si vous utilisez des UI
using System.Collections;


public class FindingKey : MonoBehaviour
{
    private LetterManage letterManage; 
    public AudioClip Soundeffect;
    public AudioSource SoundSource;

    public AudioSource AshSearching;

    public AudioSource MonsterEnding;
    public AudioSource Stinger;

    public GameObject monster;
    public GameObject black;



    private float time = 0f;


    void Start()
    {
        if (SoundSource == null)
        {
            SoundSource = gameObject.AddComponent<AudioSource>();
        }
        SoundSource.clip = Soundeffect;
        GetComponent<SpawningMonster>().enabled = false;


        // Désactiver les GameObjects au début
        monster.SetActive(false);
        black.SetActive(false);
    }

    void Update()
    {
        GameObject letterObject = GameObject.Find("Letter(Clone)");
        if (letterObject == null) return;
        letterManage = letterObject.GetComponent<LetterManage>();
        if (letterManage == null) return;


        if (letterManage.dongplayed)
        {
            time += Time.deltaTime;
            SoundSource.Play();
            StartCoroutine(HapticFeedbackRoutine());
            //Activer le script du Spawn du monstre
            GetComponent<SpawningMonster>().enabled = true;

            letterManage.dongplayed = false;
        }

        if (time >= 15f)
        {
            AshSearching.Play();
        }

        if (time >= 30f)
        {
            // Désactiver le script du Spawn du monstre
            GetComponent<SpawningMonster>().enabled = false;
            // Arrêter la musique de recherche
            AshSearching.Stop();
            // Arrêter les vibrations
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);

            StartCoroutine(ShowEndingPanel());

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

        black.SetActive(true);
        yield return new WaitForSeconds(4f);
        MonsterEnding.Play();
        yield return new WaitForSeconds(MonsterEnding.clip.length+2f);
        monster.SetActive(true);
        yield return new WaitForSeconds(2f);

    }


}
