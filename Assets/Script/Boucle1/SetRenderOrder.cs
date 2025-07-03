using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class SetRenderOrder : MonoBehaviour
{
    public string sortingLayerName = "ParticlesFace";
    public int sortingOrder = 100;

    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        rend.sortingLayerName = sortingLayerName;
        rend.sortingOrder = sortingOrder;
    }
}