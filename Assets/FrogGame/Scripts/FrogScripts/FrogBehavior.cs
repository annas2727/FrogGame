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
        Debug.Log("JumpPadLayer: " + JumpPadLayer);

        currentBehavior = BehaviorState.CanJump;
        StartCoroutine(JumpToRandomPadRoutine());
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
                //Got to the end
                transform.position = PJ_targetPos;
                transform.rotation = PJ_targetRotation;
                ClaimReservedPad();
                currentBehavior = BehaviorState.CanJump;
            }
            
        }
        else PJ_elapsedTime = 0f;
        #endregion
    }

}
