using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class Seq4_MonsterSpawn : MonoBehaviour
{
    public GameObject monstrePrefab;
    public float spawnDistance = 2.0f;
    public float delay = 45f;
    public AudioClip finalLine;

    private Transform playerHead;

    void Start()
    {
        playerHead = GameObject.Find("CenterEyeAnchor").transform;
        Invoke(nameof(SpawnMonstre), delay);
    }

    void SpawnMonstre()
{
    Vector3 spawnPos = playerHead.position + (-playerHead.right * spawnDistance);
    spawnPos.y = playerHead.position.y - 0.3f;

    GameObject monstre = Instantiate(monstrePrefab, spawnPos, Quaternion.identity);
    monstre.transform.LookAt(playerHead.position);

    // 🎯 Activer l'IK
    IKHeadLookAt ik = monstre.GetComponent<IKHeadLookAt>();
    if (ik != null && playerHead != null)
    {
        ik.targetToLookAt = playerHead;
        ik.StartLooking();
    }

    // 🎧 Jouer la voix
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
