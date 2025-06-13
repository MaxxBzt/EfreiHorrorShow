using UnityEngine;

public class keys : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioSource Keysound;
    void Start()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Keysound.Play();
    }
}
