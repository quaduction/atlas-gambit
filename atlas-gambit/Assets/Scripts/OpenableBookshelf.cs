using UnityEngine;

public class MeshTriggerDetector : MonoBehaviour
{
    [SerializeField] private Collider targetCollider;
    [SerializeField] private Animator animator;
    [SerializeField] private string triggerParameter = "PlayBookshelf";

    void OnTriggerEnter(Collider other)
    {
        if (targetCollider != null && other == targetCollider && animator != null)
        {
            animator.SetTrigger(triggerParameter);
        }
    }
}