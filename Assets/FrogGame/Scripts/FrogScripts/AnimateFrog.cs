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

    void Update()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        
        if ((state.IsName("Idle") || state.IsName("Croak")) && state.normalizedTime >= 1f)
        {
            Idle();
        }
    }

    public void Idle() {
        int randomIdle = Random.Range(0, 4); 

        if (randomIdle > 1)
            animator.SetTrigger("Idle");
        else
            animator.SetTrigger("Croak");
    }

    public void Jump() => animator.SetTrigger("Jump");
    public void LandJump() => animator.SetTrigger("J_Land");
    public void Pickup() => animator.SetTrigger("Pickup");
    public void PickupMidair() => animator.SetTrigger("P_Midair");
    public void LandPickup() => animator.SetTrigger("P_Land");
    public void Swim() => animator.SetTrigger("Swim");
    public void SwimLegKick() => animator.SetTrigger("S_LegKick");
    public void SwimSym() => animator.SetTrigger("S_Sym");
    public void Kiss() => animator.SetTrigger("Kiss");

    public void StartSwimming()
    {
        ResetTriggers();
        swimCoroutine = StartCoroutine(SwimLoop());
    }
    
    public void ResetTriggers()
    {
        animator.ResetTrigger("Jump");
        animator.ResetTrigger("J_Land");
        animator.ResetTrigger("Pickup");
        animator.ResetTrigger("P_Midair");
        animator.ResetTrigger("P_Land");
        animator.ResetTrigger("Swim");
        animator.ResetTrigger("S_Sym");
        animator.ResetTrigger("S_LegKick");
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Kiss");
    }
    public void StopSwimming()
    {
        if (swimCoroutine != null)
        {
            StopCoroutine(swimCoroutine);
            swimCoroutine = null;
            ResetTriggers();
            animator.SetTrigger("Idle");
        }
    }

    IEnumerator SwimLoop()
    {
        ResetTriggers();
        Swim();
        yield return null;
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("S_Sym"));

        while (true)
        {
            // wait for normalizedTime to reset first, then wait for near finish
            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.85f);
            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f);
            
            SwimLegKick();
            yield return null;
            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).IsName("S_LegKick"));

            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.85f);
            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f);

            SwimSym();
            yield return null;
            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).IsName("S_Sym"));
        }
    }
}
