using UnityEngine;

public class keys : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioClip Keyclip;
    public AudioSource Keysound;
    void Start()
    {
        Keysound.clip = Keyclip;
        Keysound.playOnAwake = false;
        Keysound.loop = false;

        
    }

    void OnCollisionEnter(Collision collision)
    {
        Keysound.Play();
    }
}
