using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBetweenLilypad : MonoBehaviour
{
    [Header("Lily Pad Settings")]
    public float searchRadius = 10f;
    public LayerMask lilyPadLayer;
    public LayerMask frogLayer;

    [Header("Wait Time")]
    public float minWaitTime = 1f;
    public float maxWaitTime = 1f;

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
                jumpScript.JumpToPad(targetPad);
                currentPad = targetPad;
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

            // Don't choose the pad we're currently standing on
            bool isNotLastPad = currentPad == null || pad != currentPad;

            // Don't choose pads too small
            bool isLargeEnough = (pad.transform.localScale.x/0.8f) >= (transform.localScale.x/0.25f);

            // Don't choose 
            Collider[] frogsOnPad = Physics.OverlapSphere(
                pad.transform.position,
                0.5f,
                frogLayer
            );
            bool isOccupied = frogsOnPad.Length > 0;

            if (isNotLastPad && isLargeEnough && !isOccupied)
            { 
                validPads.Add(hit.gameObject);
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
