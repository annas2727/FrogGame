using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JumpSpot : MonoBehaviour
{
    public bool isOccupied = false;
    public bool isReserved = false;
    public bool sizeScaled = true;
    public float size = 1f;
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
