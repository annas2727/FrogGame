using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBreed : MonoBehaviour
{
    [SerializeField] private Stats statsConfig;

    public BreedSpot connectedBreedSpot;

    public int breedingPhase = 0; //0=Not breeding, 1=Is trying to breed, 2=Moments before 3=Breed
    private float breedingPhaseTime = 0f; //How long in each phase

    public bool canBreedAgain = true; //For breed cooldown

    IEnumerator BreedCooldown()
    {
        canBreedAgain = false;
        yield return new WaitForSeconds(statsConfig.BreedCooldown);
        canBreedAgain = true;
    }

    public void ConnectBreedSpot(BreedSpot myBreedSpot)
    {
        connectedBreedSpot = myBreedSpot;
        connectedBreedSpot.myFrog = this.gameObject;
        connectedBreedSpot.Reserve();

    }
    public void LeaveBreedSpot()
    {
        if (connectedBreedSpot != null)
        {
            connectedBreedSpot.myFrog = null;
            connectedBreedSpot.Leave();
            Debug.Log("Frog leave breeding spot");
        }
        connectedBreedSpot = null;
    }

    public void ChangeBreedPhase(int phase)
    {
        Debug.Log("Phase change to " +  phase);
        breedingPhase = phase;
        breedingPhaseTime = 0f;
        if (phase == 0)
        {
            LeaveBreedSpot();
            GetComponent<FrogChooseJump>().isOccupied = false; //Make frog leave
        }
        else if (phase == 1)
        {
            StartCoroutine(TurnToPartner());
        }
        else if (phase == 2)
        {

        }
        else if (phase == 3)
        {
            GetComponent<AnimateFrog>().Kiss();
        }
    }

    private void Update()
    {
        if (breedingPhase != 0)
        {
            GetComponent<FrogChooseJump>().isOccupied = true;
            breedingPhaseTime += Time.deltaTime;
        }
        if (breedingPhase == 1)
        {
            if (connectedBreedSpot.Partner.myFrog != null) //Found a partner
            {
                ChangeBreedPhase(2);
            }
            if (breedingPhaseTime >= statsConfig.BreedAnnoyance)
            {
                ChangeBreedPhase(0);
            }
        }
        else if (breedingPhase == 2)
        {
            if (breedingPhaseTime >= statsConfig.BreedChickenOut)
            {
                GetComponent<FrogDrag>().CanBeDragged = false; //No interuptions now
                ChangeBreedPhase(3);
            }
        }
        else if (breedingPhase == 3)
        {
            if (breedingPhaseTime >= 2.5f)
            {
                GetComponent<FrogDrag>().CanBeDragged = true; //We done
                //Breed make egg
                ChangeBreedPhase(0);
            }
        }
    }

    public void StartTryBreeding()
    {
        ChangeBreedPhase(1);//Start trying
    }

    IEnumerator TurnToPartner()
    {
        Transform target = connectedBreedSpot.Partner.transform;
        Vector3 padNormal = connectedBreedSpot.transform.up;
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
}