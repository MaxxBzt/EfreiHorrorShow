using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SpawnMonster : MonoBehaviour
{
    public GameObject monstrePrefab;   
    public float spawnDelay = 30f;  
    public float spawnDistance = 2f;    
    public AudioClip monsterLine; 

    private Transform playerHead;

    void Start()
    {
        playerHead = GameObject.Find("CenterEyeAnchor").transform;
        StartCoroutine(DelayedSpawn());
    }

    IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        SpawnTheMonster();
    }

    void SpawnTheMonster()
    {
        // Spawn to the player's left
        Vector3 spawnPos = playerHead.position + (-playerHead.right * spawnDistance);
        spawnPos.y = playerHead.position.y - 0.3f;

        GameObject monster = Instantiate(monstrePrefab, spawnPos, Quaternion.identity);
        monster.transform.LookAt(playerHead.position); // Optional: face him

        // 🔁 Enable IK
        IKHeadLookAt ik = monster.GetComponent<IKHeadLookAt>();
        if (ik != null)
        {
            ik.targetToLookAt = playerHead;
            ik.StartLooking();
        }

        // 🔊 Play voice line
        AudioSource audio = monster.GetComponent<AudioSource>();
        if (audio != null && monsterLine != null)
        {
            audio.clip = monsterLine;
            audio.Play();
        }
    }
}
