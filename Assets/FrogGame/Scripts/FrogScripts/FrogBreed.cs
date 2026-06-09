using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBreed : MonoBehaviour
{
    [SerializeField] private Stats statsConfig;

    public BreedSpot connectedBreedSpot;

    private float breedingAnnoyanceTime = 0f; //Leave the spot time

    public bool tryBreeding = false; //Is trying to breed

    public bool canBreedAgain = true; //For breed cooldown

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

    private void Update()
    {
        if (tryBreeding)
        {
            breedingAnnoyanceTime += Time.deltaTime;
            if (connectedBreedSpot.Partner.myFrog != null) //Found a partner
            {
                //BREED
                breedingAnnoyanceTime = 0f;
            }
            if (breedingAnnoyanceTime >= statsConfig.BreedAnnoyance)
            {
                Debug.Log("Leave Breed Spot");
                LeaveBreedSpot();
                GetComponent<FrogChooseJump>().isOccupied = false; //Make frog leave
                breedingAnnoyanceTime = 0f;
                tryBreeding = false;
            }
        }
    }

    public void StartTryBreeding()
    {
        StartCoroutine(TurnToPartner());
        tryBreeding = true;
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