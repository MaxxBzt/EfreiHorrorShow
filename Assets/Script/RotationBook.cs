using UnityEngine;
using Oculus.Interaction;

public class HoldRotationSetter : MonoBehaviour
{
    public Vector3 heldRotationEuler = new Vector3(0, 0, 270);
    public Transform notebookRoot;

    private Grabbable grabbable;
    private Quaternion heldRotation;

    void Awake()
    {
        grabbable = GetComponent<Grabbable>();
        heldRotation = Quaternion.Euler(heldRotationEuler);

        if (notebookRoot == null)
        {
            // Try to find the notebook root by searching up the hierarchy
            notebookRoot = transform.parent;
        }

        if (grabbable == null)
        {
            Debug.LogError("HoldRotationSetter requires a Grabbable component on the same GameObject.");
        }

        if (notebookRoot == null)
        {
            Debug.LogError("HoldRotationSetter could not find the notebook root. Please assign it manually in the Inspector.");
        }
    }

    void OnEnable()
    {
        if (grabbable != null)
            grabbable.WhenPointerEventRaised += OnPointerEvent;
    }

    void OnDisable()
    {
        if (grabbable != null)
            grabbable.WhenPointerEventRaised -= OnPointerEvent;
    }

    private void OnPointerEvent(PointerEvent evt)
    {
        // Only set rotation once, when grab starts
       if (evt.Type == PointerEventType.Select)
        {
            Debug.Log("HOLDING it");

            if (notebookRoot != null)
            {
                notebookRoot.rotation = heldRotation;
            }
        }
    }
}
