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
    public void MakeEgg()
    {
        //ugh this is so ugly, so basically:
        //Get the parent (the unsplit BreedSpot) component <Breeding> and give it our frog and if we are the left BreedSpot
        transform.parent.GetComponent<Breeding>().ConnectFrog(myFrog, gameObject.name == "BreedSpot (L)");
    }
}
