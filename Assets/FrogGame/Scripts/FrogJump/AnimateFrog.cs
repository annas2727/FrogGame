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
        swimCoroutine = StartCoroutine(SwimLoop());
    }

    IEnumerator SwimLoop()
    {
        Swim();

        while (true)
        {
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        }

        yield return new WaitForSeconds(0.05f);

        if (Random.value < 0.5f)
            SwimLegKick();
        else
            SwimSym();
    }
}
