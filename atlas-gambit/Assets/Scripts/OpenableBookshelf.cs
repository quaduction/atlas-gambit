using GRisk.Interface;
using UnityEngine;

public class MeshTriggerDetector : MonoBehaviour
{
    public GRSound soundPlayer;

    [SerializeField] private Collider targetCollider;
    [SerializeField] private GameObject bookshelf;
    [SerializeField] private Animator animator;
    [SerializeField] private string triggerParameter = "PlayBookshelf";

    private bool opened = false;
    
    void OnTriggerEnter(Collider other)
    {
        if (!opened && targetCollider != null && other == targetCollider && animator != null)
        {
            opened = true;
            animator.SetTrigger(triggerParameter);
            soundPlayer.playsound("door-slide", bookshelf.transform);
        }
    }
}