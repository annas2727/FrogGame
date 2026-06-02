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


        //Tilted planes have smaller radius to jump from
        float tiltedmultiplier = 1f;
        if (currentPad != null && currentPad.GetComponent<JumpSpot>().isTilted)
        {
            tiltedmultiplier = 0.5f;
        }


        while (validPads.Count == 0 && searchCount < statsConfig.JumpSpotSearchNumber)
        {
            //sphere of pads to jump to, increases each search
            Collider[] hits = Physics.OverlapSphere(
                transform.position,
                (statsConfig.JumpSpotSearchRadius + (statsConfig.JumpSpotSearchRadiusAddition * searchCount)  ) * tiltedmultiplier,
                lilyPadLayer
            );


            foreach (Collider hit in hits)
            {
                GameObject pad = hit.gameObject;

                // Don't choose pads too small
                JumpSpot jumpSpotData = pad.GetComponent<JumpSpot>();
                bool isLargeEnough = (pad.transform.localScale.x / statsConfig.BaseLilypadScale) >= (transform.localScale.x / statsConfig.MaxFrogScale);

                if (isLargeEnough && !jumpSpotData.isReserved)
                {
                    validPads.Add(pad);
                    //Small frogs double chance to go onto small pads
                    if(pad.transform.localScale.x / statsConfig.BaseLilypadScale < 0.95f)
                    {
                        validPads.Add(pad);
                    }
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
        Vector3 padNormal = currentPad != null
            ? currentPad.transform.up
            : transform.up;

        Vector3 direction = Vector3.ProjectOnPlane(
            target.position - transform.position,
            padNormal
        ).normalized;

        if (direction.sqrMagnitude < 0.001f)
            yield break;

        Quaternion targetRotation = Quaternion.LookRotation(direction, padNormal);

        float turnSpeed = 8f;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * turnSpeed
            );

            yield return null;
        }

        transform.rotation = targetRotation;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
