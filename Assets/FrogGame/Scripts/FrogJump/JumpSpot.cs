using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JumpSpot : MonoBehaviour
{
    public bool isReserved = false; //Has a frog on it or jumping to it
    public bool isTilted = false; //Is a pad on a wall

    public void Reserve()
    {
        isReserved = true;
    }
    public void Leave()
    {
        isReserved = false;
    }
}
