using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBreed : MonoBehaviour
{
    [SerializeField] private Stats statsConfig;

    public LayerMask breedingPadLayer;

    IEnumerator BreedCooldown()
    {
        yield return new WaitForSeconds(statsConfig.BreedCooldown);
    }
    
    public void StartTryBreeding(GameObject HeartRock)
    {
        //FindBreedingSpot();
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
            //Go to that spot
            return null;
        }
        if (!furthestBreedSpot.isReserved)
        {
            //Go to that spot
            return null;
        }
        //To do, make them jump away quickly
        return null;
    }
    */
}