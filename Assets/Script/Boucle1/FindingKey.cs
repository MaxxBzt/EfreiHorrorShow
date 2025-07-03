using UnityEngine;

public class FindingKey : MonoBehaviour
{
    private LetterManage letterManage; 
    public AudioClip Soundeffect;
    public AudioSource SoundSource;

    void Start()
    {
        if (SoundSource == null)
        {
            SoundSource = gameObject.AddComponent<AudioSource>();
        }
        SoundSource.clip = Soundeffect;
    }

    void Update()
    {
        GameObject letterObject = GameObject.Find("Letter(Clone)");
        if (letterObject == null) return;
        letterManage = letterObject.GetComponent<LetterManage>();
        if (letterManage == null) return;


        if (letterManage.dongplayed)
        {
            SoundSource.Play();
            StartCoroutine(HapticFeedbackRoutine());
            letterManage.dongplayed = false;
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
}
