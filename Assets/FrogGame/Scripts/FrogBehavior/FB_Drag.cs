using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class FrogBehavior : MonoBehaviour
{
    #region === Drag Control ===

    private bool InDragCooldown = false;
    private bool DraggingLocked = false;
    private int DraggingLockCount = 0; //0 is unlocked, any other 
    private float DraggingLockTimer = 0f;

    public bool CanBeDragged()
    {
        return !(InDragCooldown || DraggingLocked);
    }

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

        #region ===Animation===
        GetComponent<AnimateFrog>().ResetTriggers();
        if (currentBehavior == BehaviorState.Jumping)
            GetComponent<AnimateFrog>().PickupMidair();
        else if (currentBehavior == BehaviorState.Swimming)
            GetComponent<AnimateFrog>().PickupMidair();
        else
            GetComponent<AnimateFrog>().Pickup();
        #endregion

        #region ===Behavior===
        //Behavior changes inside these
        ReleaseAllPads(); //Unreserve all pads
        ChangeBreedPhase(0); //Cancel Breeding
        disconnectFrogFromTop(); //Disconnect Frog
        
        //Fix behavior to being dragged
        currentBehavior = BehaviorState.Dragging;
        #endregion

        #region ===Math===
        Vector3 euler = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, euler.y, 0f);

        PJ_elapsedTime = 0f; //For Parabolic Jump
        JC_ElapsedTime = 0f; //For Jump Control
        PJ_startRotation = transform.rotation;
        #endregion
       
        Debug.Log("Frog has started being dragged");
    }

    public void EndDrag()
    {
        GetComponent<AnimateFrog>().ResetTriggers();
    }

    #endregion
}
