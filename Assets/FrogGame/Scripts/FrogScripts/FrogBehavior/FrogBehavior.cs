using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class FrogBehavior : MonoBehaviour
{
    [SerializeField] private Stats statsConfig;
    public enum BehaviorState { CanJump, Jumping, Swimming, Dragging, Riding, Breeding }
    public BehaviorState currentBehavior;

    private void Start()
    {
        #region ===Jump Control===
        currentBehavior = BehaviorState.CanJump;
        #endregion

        #region ===Swim Control===
        shorePointsContainer = GameObject.Find("ShoreSpots").transform;
        foreach (Transform child in shorePointsContainer)
        {
            shorePoints.Add(child);
        }
        #endregion

        #region ===Ride Control===
        myBackSpot = transform.Find("BackSpot");
        #endregion
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

        #region ===Swim Control===
        if (currentBehavior == BehaviorState.Swimming)
        {
            Vector3 dir = (SC_targetPos - transform.position);
            //dir.y = 0f;

            if (dir.sqrMagnitude < 0.1f)
            {
                currentBehavior = BehaviorState.CanJump;
                GetComponent<AnimateFrog>().ResetTriggers();
                GetComponent<AnimateFrog>().StopSwimming();
                JumpToRandomPad();
                return;
            }

            transform.position += dir.normalized * statsConfig.SwimSpeed * Time.deltaTime;

            transform.forward = dir.normalized;
        }
        #endregion

        #region ===Jump Control===
        if (currentBehavior == BehaviorState.CanJump)
        {
            JC_ElapsedTime += Time.deltaTime;
            if (JC_ElapsedTime >= JC_JumpWaitTime)
            {
                JC_ElapsedTime = 0f;
                JC_JumpWaitTime = Random.Range(statsConfig.MinIdleJumpTime, statsConfig.MaxIdleJumpTime);
                JumpToRandomPad();
                currentBehavior = BehaviorState.CanJump;
            }
        }
        #endregion

        #region ===Parabolic Jump===
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
            if (PJ_elapsedTime >= statsConfig.JumpSpeed / 3f)
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

        #region ===Ride Control===
        if (currentBehavior == BehaviorState.Riding)
        {
            //Put us on the frog's back
            transform.position = FrogOnBottom.GetComponent<FrogBehavior>().myBackSpot.position;
            //Rotated to be flush against back
            transform.rotation = FrogOnBottom.rotation * Quaternion.Euler(-8f, 0f, 0f);
        }
        #endregion
    }
}
