using UnityEngine;
using System.Collections;

public class AnimateFrog : MonoBehaviour
{
    private Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Jump", false);
        animator.SetBool("Idle", true);
    }

    public void SetAnimationState(string state)
    {
        animator.SetBool("Idle", state == "Idle");
        animator.SetBool("Jump", state == "Jump");
    }
}
