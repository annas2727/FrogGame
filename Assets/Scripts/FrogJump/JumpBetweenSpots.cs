using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBetweenSpots : MonoBehaviour
{
    [SerializeField] private Stats statsConfig;

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
                targetPad.GetComponent<JumpSpot>().Reserve(); //Reserve New Pad
                yield return StartCoroutine(TurnToPad(targetPad.transform));
                yield return new WaitForSeconds(1.5f);
                jumpScript.JumpToPad(targetPad);
                if (currentPad != null)
                    currentPad.GetComponent<JumpSpot>().Leave(); //Unreserve Current Pad
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

    IEnumerator TurnToPad(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        if (direction == Vector3.zero)
            yield break;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        float turnSpeed = 8f; // adjust for faster/slower turning

        while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * turnSpeed
            );

            yield return null;
        }

        // snap final alignment (prevents tiny drift)
        transform.rotation = targetRotation;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
