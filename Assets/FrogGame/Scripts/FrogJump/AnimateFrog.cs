using UnityEngine;
using System.Collections;

public class AnimateFrog : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    public void Jump()
    {
        animator.SetTrigger("Jump");
    }

    public void Pickup()
    {
        animator.SetTrigger("Pickup");
    }

    public void Idle()
    {
        animator.SetTrigger("Idle");
    }

    public void Croak()
    {
        animator.SetTrigger("Croak");
    }
}
