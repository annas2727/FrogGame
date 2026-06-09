using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBreed : MonoBehaviour
{
    [SerializeField] private Stats statsConfig;

    public LayerMask breedingPadLayer;
    public BreedSpot connectedBreedSpot;

    private float breedingAnnoyanceTime = 0f;
    public bool canBreedAgain = true;

    IEnumerator BreedCooldown()
    {
        canBreedAgain = false;
        yield return new WaitForSeconds(statsConfig.BreedCooldown);
        canBreedAgain = true;
    }

    IEnumerator BreedAnnoyanceTimer()
    {
        yield return new WaitForSeconds(1f);
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
        }
        connectedBreedSpot = null;
    }

    public void StartTryBreeding()
    {
        StartCoroutine(TurnToPartner());
        while(breedingAnnoyanceTime < statsConfig.BreedAnnoyance)
        {
            breedingAnnoyanceTime += Time.deltaTime;
            if(connectedBreedSpot.Partner.myFrog != null)
            {
                break;
            }
        }
        if(breedingAnnoyanceTime >= statsConfig.BreedAnnoyance)
        {
            LeaveBreedSpot();
            GetComponent<FrogChooseJump>().isOccupied = false; //Make frog leave
        }
        else //Found a partner
        {
            //BREED
        }
        breedingAnnoyanceTime = 0f;
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