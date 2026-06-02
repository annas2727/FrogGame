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
            float waitTime = Random.Range(statsConfig.MinIdleJumpTime, statsConfig.MaxIdleJumpTime);
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
        List<GameObject> validPads = new List<GameObject>();
        int searchCount = 0;

        while (validPads.Count == 0 && searchCount < statsConfig.JumpSpotSearchNumber)
        {
            Collider[] hits = Physics.OverlapSphere(
                transform.position,
                statsConfig.JumpSpotSearchRadius + (statsConfig.JumpSpotSearchRadiusAddition * searchCount),
                lilyPadLayer
            );

            foreach (Collider hit in hits)
            {

                GameObject pad = hit.gameObject;

                // Don't choose pads too small
                JumpSpot jumpSpotData = pad.GetComponent<JumpSpot>();
                Debug.Log(jumpSpotData != null);
                bool isLargeEnough = (pad.transform.localScale.x / statsConfig.BaseLilypadScale) >= (transform.localScale.x / statsConfig.MaxFrogScale);


                if (isLargeEnough && !jumpSpotData.isReserved)
                {
                    validPads.Add(pad);
                }
            }
            searchCount += 1;
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
