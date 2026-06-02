using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JumpSpot : MonoBehaviour
{
    public bool isReserved = false; //Has a frog on it or jumping to it
    public bool sizeScaled = true; //Use the scale of the object as the size?
    public float size = 1f; //What size of frog can be on the pad

    public bool isTilted = false; //Is a pad on a wall, needs to jump to "nextPad"
    [SerializeField] private GameObject nextPad; //The next pad to jump to
    
    public GameObject getNextPad()
    {
        return nextPad;
    }
    public float getSize()
    {
        if (sizeScaled)
            return transform.localScale.x;
        return size;
    }
    public void Reserve()
    {
        isReserved = true;
    }
    public void Leave()
    {
        isReserved = false;
    }
}
