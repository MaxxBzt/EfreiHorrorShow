using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class AshVoiceManager : MonoBehaviour
{
    public PlayableDirector fallbackTimeline;
    public List<AshVoiceID> allVoices = new List<AshVoiceID>
    {
        AshVoiceID.WhereAreYouGuys,
        AshVoiceID.WhoIsHere,
        AshVoiceID.Taylor,
        AshVoiceID.Red
    };

    private HashSet<AshVoiceID> triggeredVoices = new HashSet<AshVoiceID>();
    private bool fallbackStarted = false;

    public GameObject monstrePrefab;
    public float spawnDistance = 2f;
    public AudioClip finalLine;
    public float fallbackDelay = 30f;

    private bool timelinePlayed = false;
    private Transform playerHead;

    void Start()
    {
        playerHead = GameObject.Find("CenterEyeAnchor").transform;
        Invoke(nameof(CheckForFallback), fallbackDelay);
    }

    public void RegisterVoiceTrigger(AshVoiceID id)
    {
        if (!triggeredVoices.Contains(id))
            triggeredVoices.Add(id);
    }

    void CheckForFallback()
    {
        if (triggeredVoices.Count < allVoices.Count)
        {
            timelinePlayed = true;
            fallbackTimeline.Play();
            StartCoroutine(WaitForTimelineThenSpawn());
        }
        else
        {
            // Toutes les voix ont été jouées, spawn direct après 5s
            Invoke(nameof(SpawnMonstre), 5f);
        }
    }

    IEnumerator WaitForTimelineThenSpawn()
    {
        yield return new WaitForSeconds((float)fallbackTimeline.duration + 1f);
        SpawnMonstre();
    }

    public bool ShouldPlayVoiceInTimeline(AshVoiceID id)
    {
        return !triggeredVoices.Contains(id);
    }

    void SpawnMonstre()
    {
        Vector3 spawnPos = playerHead.position + (-playerHead.right * spawnDistance);
        spawnPos.y = playerHead.position.y - 0.3f;

        GameObject monstre = Instantiate(monstrePrefab, spawnPos, Quaternion.identity);
        monstre.transform.LookAt(playerHead.position);

        AudioSource audio = monstre.GetComponent<AudioSource>();
        if (audio != null && finalLine != null)
        {
            audio.clip = finalLine;
            audio.Play();
        }

        StartCoroutine(TransitionToAR());

    }

    IEnumerator TransitionToAR()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("AR_MainGame");
    }
}
