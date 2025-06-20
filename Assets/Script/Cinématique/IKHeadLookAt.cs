using UnityEngine;

public class IKHeadLookAt : MonoBehaviour
{
    public Transform targetToLookAt; // Main Camera (VR)
    public float lookAtWeight = 1.0f;

    private Animator animator;
    private bool shouldLook = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator == null || targetToLookAt == null) return;

        if (shouldLook)
        {
            animator.SetLookAtWeight(lookAtWeight);
            animator.SetLookAtPosition(targetToLookAt.position);
        }
        else
        {
            animator.SetLookAtWeight(0f);
        }
    }

    public void StartLooking()
    {
        shouldLook = true;
    }

    public void StopLooking()
    {
        shouldLook = false;
    }
}