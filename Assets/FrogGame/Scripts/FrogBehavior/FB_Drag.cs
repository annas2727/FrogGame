using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class FrogBehavior : MonoBehaviour
{
    #region === Drag Control ===

    public bool InDragCooldown = true;
    public bool DraggingLocked = false;
    private int DraggingLockCount = 0; //0 is unlocked, any other 
    private float DraggingLockTimer = 0f;

    private void LockDrag()
    {
        DraggingLocked = true;
        DraggingLockCount += 1;
        DraggingLockTimer = 0f;
    }

    private void UnLockDrag()
    {
        DraggingLockCount -= 1;
        DraggingLockTimer = 0f;
        if(DraggingLockCount <= 0)
        {
            DraggingLocked = false;
            DraggingLockCount = 0;
        }
    }

    IEnumerator StartDragCooldown()
    {
        yield return new WaitForSeconds(statsConfig.DragCooldown);
        InDragCooldown = false;
    }

    public void BeginDrag()
    {
        StartCoroutine(StartDragCooldown()); //Start DragCooldown

        currentBehavior = BehaviorState.Dragging;

        ReleaseAllPads(); //Unreserve all pads
        ChangeBreedPhase(0); //Cancel Breeding
        //GetComponent<FrogTop>().disconnectFrogFromTop();

        Vector3 euler = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, euler.y, 0f);

        PJ_elapsedTime = 0f; //For Parabolic Jump
        JC_ElapsedTime = 0f; //For Jump Control
        PJ_startRotation = transform.rotation;

        GetComponent<AnimateFrog>().ResetTriggers();

        if(currentBehavior == BehaviorState.Jumping)
            GetComponent<AnimateFrog>().PickupMidair();
        else if(currentBehavior == BehaviorState.Swimming)
            GetComponent<AnimateFrog>().PickupMidair();
        else
            GetComponent<AnimateFrog>().Pickup();

        Debug.Log("In frog drag mode");
    }

    public void EndDrag()
    {
        GetComponent<AnimateFrog>().ResetTriggers();
    }

    #endregion
}
