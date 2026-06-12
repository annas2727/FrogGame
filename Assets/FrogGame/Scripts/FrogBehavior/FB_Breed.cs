using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class FrogBehavior : MonoBehaviour
{
    #region === Breed Control ===
    public BreedSpot BC_CurrentBreedPad;

    public int breedingPhase = 0; //0=Not breeding, 1=Is trying to breed, 2=Moments before 3=Breed
    private float breedingPhaseTime = 0f; //How long in each phase

    public bool InBreedCooldown = true; //For breed cooldown
    public bool isAdult = false;
    #endregion

    //Methods

    #region === Breed Control ===
    IEnumerator BreedCooldown()
    {
        InBreedCooldown = false;
        yield return new WaitForSeconds(statsConfig.BreedCooldown);
        InBreedCooldown = true;
    }

    public void ConnectBreedSpot(BreedSpot myBreedSpot)
    {
        BC_CurrentBreedPad = myBreedSpot;
        BC_CurrentBreedPad.myFrog = this.gameObject;
        BC_CurrentBreedPad.Reserve();
            
    }
    public void LeaveBreedSpot()
    {
        if (BC_CurrentBreedPad != null)
        {
            BC_CurrentBreedPad.myFrog = null;
            BC_CurrentBreedPad.Leave();
            Debug.Log("Frog leave breeding spot");
        }
        BC_CurrentBreedPad = null;
    }

    private void ChangeBreedPhase(int phase)
    {
        breedingPhase = phase;
        breedingPhaseTime = 0f;
        switch (phase)
        {
            case 0:
                LeaveBreedSpot();
                break;
            case 1:
                StartCoroutine(TurnToPartner());
                break;
            case 2:
                break;
            case 3:
                GetComponent<AnimateFrog>().Kiss();
                break;

        }
    }

    public void StartTryBreeding()
    {
        ChangeBreedPhase(1);//Start trying
    }

    IEnumerator TurnToPartner()
    {
        Transform target = BC_CurrentBreedPad.Partner.transform;
        Vector3 padNormal = BC_CurrentBreedPad.transform.up;
        Vector3 direction = Vector3.ProjectOnPlane(
            target.position - transform.position,
            padNormal
        ).normalized;

        if (direction.sqrMagnitude < 0.001f)
            yield break;
        Quaternion targetRotation = Quaternion.LookRotation(direction, padNormal);

        float turnSpeed = 8f;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * turnSpeed
            );

            yield return null;
        }

        transform.rotation = targetRotation;
    }
    #endregion
}
