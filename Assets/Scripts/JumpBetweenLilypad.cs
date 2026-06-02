using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBetweenLilypad : MonoBehaviour
{
    [Header("Lily Pad Settings")]
    public float searchRadius = 10f;
    public LayerMask lilyPadLayer;

    [Header("Wait Time")]
    public float minWaitTime = 1f;
    public float maxWaitTime = 5f;

    private FrogJump jumpScript;

    private void Start()
    {
        jumpScript = GetComponent<FrogJump>();
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
            // Don't choose the pad we're currently standing on
            if (hit.gameObject != gameObject)
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
