using UnityEngine;
using System.Collections;

public class AnimateFrog : MonoBehaviour
{
    private Animator animator;
    private Transform armature;
    private Coroutine swimCoroutine;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("Idle");
    }


    public void Jump() => animator.SetTrigger("Jump");
    public void LandJump() => animator.SetTrigger("J_Land");
    public void Pickup() => animator.SetTrigger("Pickup");
    public void PickupMidair() => animator.SetTrigger("P_Midair");
    public void LandPickup() => animator.SetTrigger("P_Land");
    public void Idle() => animator.SetTrigger("Idle");
    public void Croak() => animator.SetTrigger("Croak");
    public void Swim() => animator.SetTrigger("Swim");
    public void SwimLegKick() => animator.SetTrigger("S_LegKick");
    public void SwimSym() => animator.SetTrigger("S_Sym");

    public void StartSwimming()
    {
        //reset all triggers
        
        animator.ResetTrigger("P_Midair");
        animator.ResetTrigger("P_Land");
        animator.ResetTrigger("Pickup");
        animator.ResetTrigger("Idle");
        animator.SetTrigger("Swim");
        swimCoroutine = StartCoroutine(SwimLoop());
    }
    
    public void StopSwimming()
    {
        if (swimCoroutine != null)
        {
            StopCoroutine(swimCoroutine);
            swimCoroutine = null;
            Debug.Log("Stopped swimming");
            Idle();
        }
    }
    IEnumerator SwimLoop()
    {
        Swim(); // triggers Idle → S_Sym via your existing transition
        yield return null;
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("S_Sym"));

        while (true)
        {
            // wait for S_Sym to nearly finish, then kick
            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f);
            SwimLegKick();
            yield return null;
            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).IsName("S_LegKick"));

            // wait for S_LegKick to nearly finish, then sym
            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f);
            SwimSym();
            yield return null;
            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).IsName("S_Sym"));
        }
    }
    void Update()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
    }
}
