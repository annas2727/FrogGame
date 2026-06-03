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
        animator.SetBool("J_Takeoff", state == "J_Takeoff");
        animator.SetBool("J_Mid", state == "J_Mid");
        animator.SetBool("J_Land", state == "J_Land");
        animator.SetBool("P_Takeoff", state == "P_Takeoff");
        animator.SetBool("P_Mid", state == "P_Mid");
        animator.SetBool("P_Land", state == "P_Land");
        animator.SetBool("Swim", state == "Swim");
        animator.SetBool("Croak", state == "Croak");

    }
}
