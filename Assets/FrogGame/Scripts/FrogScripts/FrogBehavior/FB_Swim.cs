using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class FrogBehavior : MonoBehaviour
{
    #region ===Swim Control===
    private Transform shorePointsContainer;
    private List<Transform> shorePoints = new List<Transform>();
    private Vector3 SC_targetPos;
    #endregion

    //Methods

    #region ===Swim Control===
    public void StartSwimming()
    {
        GetComponent<AnimateFrog>().StartSwimming();
        SC_targetPos = FindClosestShorePoint();
        currentBehavior = BehaviorState.Swimming;
    }

    private Vector3 FindClosestShorePoint()
    {
        Transform closest = null;
        float bestDist = float.MaxValue;

        foreach (Transform point in shorePoints)
        {
            float dist = (point.position - transform.position).sqrMagnitude;

            if (dist < bestDist)
            {
                bestDist = dist;
                closest = point;
            }
        }

        return closest.position;
    }
    #endregion
}
