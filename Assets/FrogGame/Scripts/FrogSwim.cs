using System.Collections.Generic;
using UnityEngine;

public class FrogSwim : MonoBehaviour
{
    public bool isSwimming;

    public float swimSpeed = 2f;
    public Transform shorePointsContainer;
    public List<Transform> shorePoints = new List<Transform>();

    void Start()
    {
        foreach (Transform child in shorePointsContainer)
        {
            shorePoints.Add(child);
        }
    }

    void Update()
    {
        if (isSwimming)
        {
            SwimToShore();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision Enter");
        if (other.CompareTag("Water"))
        {
            Debug.Log("We are swimming now boys");
            isSwimming = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isSwimming = false;
        }
    }

    void SwimToShore()
    {
        Vector3 target = FindClosestShorePoint();

        Vector3 dir = (target - transform.position);
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.1f)
        {
            //ExitWater();
            return;
        }

        transform.position += dir.normalized * swimSpeed * Time.deltaTime;

        transform.forward = dir.normalized;
    }

    Vector3 FindClosestShorePoint()
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
}