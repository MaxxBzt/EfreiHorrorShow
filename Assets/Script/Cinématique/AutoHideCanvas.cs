using UnityEngine;

public class AutoHideCanvas : MonoBehaviour
{
    public Canvas canvas;

    void Start()
    {
        Invoke(nameof(Hide), 5f);
    }

    void Hide()
    {
        canvas.enabled = false;
    }
}
