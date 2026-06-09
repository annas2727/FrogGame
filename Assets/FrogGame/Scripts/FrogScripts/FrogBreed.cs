using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBreed : MonoBehaviour
{
    [SerializeField] private Stats statsConfig;

    public LayerMask breedingPadLayer;
    public BreedSpot connectedBreedSpot;

    IEnumerator BreedCooldown()
    {
        yield return new WaitForSeconds(statsConfig.BreedCooldown);
    }

    public void ConnectBreedSpot(BreedSpot myBreedSpot)
    {
        connectedBreedSpot = myBreedSpot;
        connectedBreedSpot.Reserve();

    }
    public void LeaveBreedSpot()
    {
        if(connectedBreedSpot != null)
            connectedBreedSpot.Leave();
        connectedBreedSpot = null;
    }

    public void StartTryBreeding()
    {
        StartCoroutine(TurnToPartner());
        //myBreedSpot.GetComponent<BreedSpot>().Reserve();
        //FindBreedingSpot(HeartRock);
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

    /*
    GameObject FindBreedingSpot(GameObject HeartRock)
    {
        BreedSpot BreedSpotL = HeartRock.transform.Find("BreedSpot (L)").GetComponent<BreedSpot>();
        BreedSpot BreedSpotR = HeartRock.transform.Find("BreedSpot (R)").GetComponent<BreedSpot>();

        float distL = Vector3.Distance(transform.position, BreedSpotL.transform.position);
        float distR = Vector3.Distance(transform.position, BreedSpotR.transform.position);

        BreedSpot closestBreedSpot = distL < distR ? BreedSpotL : BreedSpotR;
        BreedSpot furthestBreedSpot = distL >= distR ? BreedSpotR : BreedSpotL;

        if (!closestBreedSpot.isReserved)
        {
            
            return null;
        }
        if (!furthestBreedSpot.isReserved)
        {
            //Go to that spot
            return null;
        }
        frog.GetComponent<FrogChooseJump>().isOccupied = false;
        return null;
    }
    */
}