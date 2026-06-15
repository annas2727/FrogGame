using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class FrogBehavior : MonoBehaviour
{
    #region === Layer masks ===
    [SerializeField] private LayerMask FrogLayer;
    [SerializeField] private LayerMask JumpPadLayer;
    #endregion

    #region=== Jump Control ===
    private GameObject JC_CurrentJumpPad;
    private GameObject JC_CurrentReservedJumpPad;

    private float JC_JumpWaitTime = 0f;
    private float JC_ElapsedTime = 0f;
    #endregion

    #region === Parabolic Jump ===
    private Vector3 PJ_startPos;
    public Vector3 PJ_targetPos;

    private Quaternion PJ_startRotation;
    private Quaternion PJ_targetRotation;

    private Vector3 PJ_horizontalVelocity;
    private float PJ_verticalVelocity;

    private float PJ_elapsedTime;
    #endregion

    //Methods

    #region === Jump Pad Control ===

    private void ReleaseAllPads()
    {
        ReleaseClaimedPad();
        ReleaseReservedPad();
    }

    private void ReleaseClaimedPad()
    {
        if (JC_CurrentJumpPad != null)
            JC_CurrentJumpPad.GetComponent<JumpSpot>().Leave();
    }

    private void ReleaseReservedPad()
    {
        if (JC_CurrentReservedJumpPad != null)
            JC_CurrentReservedJumpPad.GetComponent<JumpSpot>().Leave();
    }

    private void ClaimPad(GameObject pad)
    {
        if (pad != null)
        {
            pad.GetComponent<JumpSpot>().Reserve();
            JC_CurrentJumpPad = pad;
        }
    }

    private void ReservePad(GameObject pad)
    {
        if (pad != null)
        {
            pad.GetComponent<JumpSpot>().Reserve();
            JC_CurrentReservedJumpPad = pad;
        }
    }

    private void ClaimReservedPad()
    {
        if (JC_CurrentReservedJumpPad != null)
        {
            JC_CurrentReservedJumpPad.GetComponent<JumpSpot>().Reserve();
            JC_CurrentJumpPad = JC_CurrentReservedJumpPad;
        }
    }

    #endregion

    #region === Jump Control ===
    public void JumpToRandomPad() //Can be called from outside to force frog to jump away
    {
        JC_ElapsedTime = 0f;
        currentBehavior = BehaviorState.CanJump; //If called from outside routine
        GameObject targetPad = ChooseRandomPad();

        if (targetPad != null)
        {
            Debug.Log("Target pad found, trying to jump to");
            StartCoroutine(JumpToPad(targetPad));
        }
    }

    private IEnumerator JumpToPad(GameObject targetPad)
    {
        ReservePad(targetPad); //Reserve New Pad
        yield return StartCoroutine(TurnToPad(targetPad.transform));
        ParabolicJumpToPad(targetPad);
        ReleaseClaimedPad(); //Unreserve Current Pad
    }

    public IEnumerator TurnToPad(Transform target)
    {
        Vector3 padNormal = JC_CurrentJumpPad != null ? JC_CurrentJumpPad.transform.up : transform.up;

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

    private GameObject ChooseRandomPad()
    {
        Debug.Log("Choose Random Pad Runs");
        List<GameObject> validPads = new List<GameObject>();
        int searchCount = 0;

        //Tilted planes have smaller radius to jump from
        float tiltedmultiplier = 1f;
        if (JC_CurrentJumpPad != null && JC_CurrentJumpPad.GetComponent<JumpSpot>().isTilted)
        {
            tiltedmultiplier = statsConfig.TiltedPadMulti;
        }


        while (validPads.Count == 0 && searchCount < statsConfig.JumpSpotSearchNumber)
        {
            //sphere of pads to jump to, increases each search
            Collider[] hits = Physics.OverlapSphere(
                transform.position,
                (statsConfig.JumpSpotSearchRadius + (statsConfig.JumpSpotSearchRadiusAddition * searchCount)) * tiltedmultiplier,
                JumpPadLayer
            );


            foreach (Collider hit in hits)
            {
                Debug.Log("Pad hit");
                GameObject pad = hit.gameObject;

                // Don't choose pads too small
                JumpSpot jumpSpotData = pad.GetComponent<JumpSpot>();
                if( jumpSpotData == null)
                    Debug.Log("Jumpspotnull: " +  hit.name);
                bool isLargeEnough = (pad.transform.lossyScale.x / statsConfig.BaseLilypadScale) >= (transform.lossyScale.x / statsConfig.MaxFrogScale);
                Debug.Log("Pad big enough: " + isLargeEnough);
                if (isLargeEnough && !jumpSpotData.isReserved)
                {
                    validPads.Add(pad);
                    //Small frogs double chance to go onto small pads
                    if (pad.transform.lossyScale.x / statsConfig.BaseLilypadScale < 0.95f)
                    {
                        validPads.Add(pad);
                    }
                }
            }
            searchCount += 1;
        }

        if (validPads.Count == 0)
            return null;
        Debug.Log("Random pad gotten");
        int randomIndex = Random.Range(0, validPads.Count);
        return validPads[randomIndex];
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (statsConfig != null)
        {
            Gizmos.DrawWireSphere(transform.position, statsConfig.JumpSpotSearchRadius);
        }
    }

    #endregion

    #region === Parabolic Jump ===
    private void ParabolicJumpToPad(GameObject pad)
    {
        if (currentBehavior != BehaviorState.CanJump)
            return;
        currentBehavior = BehaviorState.Jumping;
        GetComponent<AnimateFrog>().Jump();
        PJ_startRotation = transform.rotation;

        // Rotate so the frog's up matches the pad's up
        PJ_targetRotation = Quaternion.FromToRotation(
            transform.up,
            pad.transform.up
        ) * transform.rotation;

        ParabolicJumpToVector3(pad.transform.position); // + Vector3.up * statsConfig.BaseLilypadyOffset

    }

    private void ParabolicJumpToVector3(Vector3 target)
    {
        currentBehavior = BehaviorState.Jumping;
        GetComponent<AnimateFrog>().Jump();

        PJ_startPos = transform.position;
        PJ_targetPos = target;
        PJ_elapsedTime = 0f;

        Vector3 displacement = PJ_targetPos - PJ_startPos;

        // Horizontal motion (XZ)
        Vector3 horizontalDisplacement = new Vector3(
            displacement.x,
            0f,
            displacement.z
        );

        PJ_horizontalVelocity = horizontalDisplacement / statsConfig.JumpSpeed;

        // Vertical motion (Y)
        PJ_verticalVelocity =
            (displacement.y - 0.5f * statsConfig.JumpGravity * statsConfig.JumpSpeed * statsConfig.JumpSpeed)
            / statsConfig.JumpSpeed;
    }
    #endregion
}
