using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBetweenSpots : MonoBehaviour
{
    [Header("Lily Pad Settings")]
    public float searchRadius = 10f;
    public LayerMask lilyPadLayer;
    public LayerMask frogLayer;

    [Header("Wait Time")]
    public float minWaitTime = 4f;
    public float maxWaitTime = 6f;

    private ParabolicJump jumpScript;

    private GameObject currentPad;

    private void Start()
    {
        jumpScript = GetComponent<ParabolicJump>();
        StartCoroutine(ChoosePadsRoutine());
    }

    IEnumerator ChoosePadsRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            GameObject targetPad = GetRandomNearbyPad();

            if (targetPad != null)
            {
                if (currentPad != null)
                    currentPad.GetComponent<JumpSpot>().Leave(); //Unreserve Current Pad
                targetPad.GetComponent<JumpSpot>().Reserve(); //Reserve New Pad
                jumpScript.JumpToPad(targetPad);
                currentPad = targetPad; //Save New Pad
            }
        }
    }

    GameObject GetRandomNearbyPad()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            searchRadius,
            lilyPadLayer
        );

        List<GameObject> validPads = new List<GameObject>();

        foreach (Collider hit in hits)
        {

            GameObject pad = hit.gameObject;

            // Don't choose pads too small
            bool isLargeEnough = (pad.transform.localScale.x / 0.8f) >= (transform.localScale.x / 0.25f);
            JumpSpot jumpSpotData = pad.GetComponent<JumpSpot>();

            if (isLargeEnough && !jumpSpotData.isReserved)
            {
                validPads.Add(pad);
            }
        }

        if (validPads.Count == 0)
            return null;

        int randomIndex = Random.Range(0, validPads.Count);
        return validPads[randomIndex];
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
