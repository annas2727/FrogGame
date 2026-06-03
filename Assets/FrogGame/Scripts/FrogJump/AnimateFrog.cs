using UnityEngine;
using System.Collections;

public class AnimateFrog : MonoBehaviour
{
    private Animator animator;
    private Transform armature;


    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("Idle");
    }


    public void Jump()
    {
        animator.SetTrigger("Jump");
    }

    public void LandJump()
    {
        animator.SetTrigger("J_Land");
    }

    public void Pickup()
    {
        animator.SetTrigger("Pickup");
    }
    public void PickupMidair()
    {
        animator.SetTrigger("P_Midair");
    }

    public void LandPickup()
    {
        animator.SetTrigger("P_Land");
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
