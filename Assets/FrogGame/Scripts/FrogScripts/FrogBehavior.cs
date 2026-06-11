using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBehavior : MonoBehaviour
{
    [SerializeField] private Stats statsConfig;
    public enum BehaviorState { CanJump, Jumping, Swimming, Dragging, Riding }
    public BehaviorState currentBehavior;

    #region === Layer masks ===
    private LayerMask FrogsLayer;
    private LayerMask JumpPadLayer;
    #endregion

    #region=== Jump Control ===
    private GameObject JC_CurrentJumpPad;
    private GameObject JC_CurrentReservedJumpPad;
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

    private void Start()
    {
        //Get Layer Masks
        LayerMask FrogsLayer = LayerMask.GetMask("Frogs");
        LayerMask JumpPadLayer = LayerMask.GetMask("JumpPad");

        currentBehavior = BehaviorState.CanJump;
    }

    private void Update()
    {


        #region Parabolic Jump
        if (currentBehavior == BehaviorState.Dragging)
        {
            PJ_elapsedTime = 0f;
            GetComponent<AnimateFrog>().PickupMidair();
            PJ_startRotation = transform.rotation;
        }
        if (currentBehavior == BehaviorState.Jumping)
        {
            PJ_elapsedTime += Time.deltaTime;

            float t = Mathf.Min(PJ_elapsedTime, statsConfig.JumpSpeed);

            Vector3 horizontalOffset = PJ_horizontalVelocity * t;

            float verticalOffset =
                PJ_verticalVelocity * t +
                0.5f * statsConfig.JumpGravity * t * t;

            transform.position =
                PJ_startPos +
                horizontalOffset +
                Vector3.up * verticalOffset;
            float rotationT = Mathf.Clamp01(PJ_elapsedTime / statsConfig.JumpSpeed);

            transform.rotation = Quaternion.Slerp(
                PJ_startRotation,
                PJ_targetRotation,
                rotationT
            );
            if (PJ_elapsedTime >= statsConfig.JumpSpeed / 2f)
            {
                GetComponent<AnimateFrog>().LandJump();
            }
            if (PJ_elapsedTime >= statsConfig.JumpSpeed)
            {
                transform.position = PJ_targetPos;
                transform.rotation = PJ_targetRotation;

                currentBehavior = BehaviorState.CanJump;
            }
            
        }
        else PJ_elapsedTime = 0f;
        #endregion
    }

    #region === Routine ===
    private IEnumerator JumpToRandomPadRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(statsConfig.MinIdleJumpTime, statsConfig.MaxIdleJumpTime);
            yield return new WaitForSeconds(waitTime);
            yield return new WaitUntil(() => currentBehavior == BehaviorState.CanJump);
            JumpToRandomPad();
        }
    }
    #endregion

    #region === Pad Control ===

    private void ReleasePadOn()
    {
        if (JC_CurrentJumpPad != null)
            JC_CurrentJumpPad.GetComponent<JumpSpot>().Leave();
    }

    private void ReleasePadReserved()
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

    #endregion

    #region === Jump Control ===
    public void JumpToRandomPad() //Can be called from outside to force frog to jump away
    {
        currentBehavior = BehaviorState.CanJump; //If called from outside routine
        GameObject targetPad = ChooseRandomPad();

        if (targetPad != null)
        {
            StartCoroutine(JumpToPad(targetPad));
        }
    }

    private IEnumerator JumpToPad(GameObject targetPad)
    {
        ReservePad(targetPad); //Reserve New Pad
        yield return StartCoroutine(TurnToPad(targetPad.transform));
        GetComponent<ParabolicJump>().JumpToPad(targetPad);
        ReleasePadOn(); //Unreserve Current Pad
        //ClaimPad(targetPad); //Save New Pad //TO DO MOVE THIS TO WHEN THE JUMP FINISHES
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
                GameObject pad = hit.gameObject;

                // Don't choose pads too small
                JumpSpot jumpSpotData = pad.GetComponent<JumpSpot>();
                bool isLargeEnough = (pad.transform.lossyScale.x / statsConfig.BaseLilypadScale) >= (transform.lossyScale.x / statsConfig.MaxFrogScale);

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

        int randomIndex = Random.Range(0, validPads.Count);
        return validPads[randomIndex];
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, statsConfig.JumpSpotSearchRadius);
    }

    #endregion

    #region === Parabolic Jump ===
    private void ParabolicJumpToPad(GameObject pad)
    {
        if (GetComponent<FrogChooseJump>().isOccupied)
            return;
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

        currentBehavior = BehaviorState.Jumping;
    }
    #endregion
}
