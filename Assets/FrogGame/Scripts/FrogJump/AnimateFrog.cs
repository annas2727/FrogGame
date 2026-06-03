using UnityEngine;
using System.Collections;

public class AnimateFrog : MonoBehaviour
{
    private Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAnimationState(string state)
    {
        animator.Play(state);
    }
}
