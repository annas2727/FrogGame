using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class FrogBehavior : MonoBehaviour
{
    [SerializeField] private Stats statsConfig;
    public enum BehaviorState { CanJump, Jumping, Swimming, Dragging, Riding, Breeding }
    public BehaviorState currentBehavior;

    #region === Breed Control ===
    public BreedSpot BC_CurrentBreedPad;

    public int breedingPhase = 0; //0=Not breeding, 1=Is trying to breed, 2=Moments before 3=Breed
    private float breedingPhaseTime = 0f; //How long in each phase

    public bool InBreedCooldown = true; //For breed cooldown
    public bool isAdult = false;
    #endregion

    private void Start()
    {
        Debug.Log("JumpPadLayer: " + JumpPadLayer);

        currentBehavior = BehaviorState.CanJump;
        StartCoroutine(JumpToRandomPadRoutine());
    }



    private void Update()
    {
        #region ===Drag Control===
        if (DraggingLocked)
        {
            DraggingLockTimer += Time.deltaTime;
            if (DraggingLockTimer > statsConfig.DragLockMaxTime)
                UnLockDrag();
        }
        #endregion

        #region ===Breed Control===
        if (currentBehavior == BehaviorState.Breeding)
        {
            breedingPhaseTime += Time.deltaTime;
            switch (breedingPhase)
            {
                case 1:
                    if (BC_CurrentBreedPad.Partner.myFrog != null) //Found a partner
                        ChangeBreedPhase(2);
                    else if (breedingPhaseTime >= statsConfig.BreedAnnoyance)
                    {
                        JumpToRandomPad(); //Make frog leave
                        ChangeBreedPhase(0);
                    }
                    break;
                case 2:
                    if (breedingPhaseTime >= statsConfig.BreedChickenOut)
                    {
                        LockDrag(); //No interuptions now
                        ChangeBreedPhase(3);
                    }
                    break;
                case 3:
                    if (breedingPhaseTime >= 2.5f)
                    {
                        UnLockDrag(); //We done
                        BC_CurrentBreedPad.MakeEgg(); //Make the egg
                        currentBehavior = BehaviorState.CanJump; //Make frog able to leave
                        ChangeBreedPhase(0);
                    }
                    break;
            }
        }
        #endregion

        #region ===Jump Randomly btw Pads===
        if (currentBehavior == BehaviorState.CanJump)
        {
            JC_ElapsedTime += Time.deltaTime;
            if (JC_ElapsedTime >= JC_JumpWaitTime)
            {
                JC_ElapsedTime = 0f;
                JC_JumpWaitTime = Random.Range(statsConfig.MinIdleJumpTime, statsConfig.MaxIdleJumpTime);
                JumpToRandomPad();
            }
        }
        #endregion

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
                //Got to the end
                transform.position = PJ_targetPos;
                transform.rotation = PJ_targetRotation;
                ClaimReservedPad(); //Claim the pad
                currentBehavior = BehaviorState.CanJump;
            }
            
        }
        else PJ_elapsedTime = 0f;
        #endregion
    }

    #region === Breed Control ===
    IEnumerator BreedCooldown()
    {
        InBreedCooldown = false;
        yield return new WaitForSeconds(statsConfig.BreedCooldown);
        InBreedCooldown = true;
    }

    public void ConnectBreedSpot(BreedSpot myBreedSpot)
    {
        connectedBreedSpot = myBreedSpot;
        connectedBreedSpot.myFrog = this.gameObject;
        connectedBreedSpot.Reserve();

    }
    public void LeaveBreedSpot()
    {
        if (connectedBreedSpot != null)
        {
            connectedBreedSpot.myFrog = null;
            connectedBreedSpot.Leave();
            Debug.Log("Frog leave breeding spot");
        }
        connectedBreedSpot = null;
    }

    private void ChangeBreedPhase(int phase)
    {
        breedingPhase = phase;
        breedingPhaseTime = 0f;
        switch (phase)
        {
            case 0:
                LeaveBreedSpot();
                break;
            case 1:
                StartCoroutine(TurnToPartner());
                break;
            case 2:
                break;
            case 3:
                GetComponent<AnimateFrog>().Kiss();
                break;

        }
    }

    public void StartTryBreeding()
    {
        ChangeBreedPhase(1);//Start trying
    }

    IEnumerator TurnToPartner()
    {
        Transform target = connectedBreedSpot.Partner.transform;
        Vector3 padNormal = connectedBreedSpot.transform.up;
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
    #endregion

}
