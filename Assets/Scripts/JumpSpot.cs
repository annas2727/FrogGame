using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JumpSpot : MonoBehaviour
{
    public bool isOccupied = false;
    public bool isReserved = false;


    public void Reserve()
    {
        isReserved = true;
    }
    public void Leave()
    {
        isReserved = false;
    }
}
