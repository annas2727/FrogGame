using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BreedSpot : MonoBehaviour
{
    public bool isReserved = false; //Has a frog on it or jumping to it
    public BreedSpot Partner;

    public GameObject myFrog;

    public void Reserve()
    {
        isReserved = true;
    }
    public void Leave()
    {
        isReserved = false;
    }
}
