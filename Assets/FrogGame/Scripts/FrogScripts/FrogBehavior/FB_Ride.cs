using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class FrogBehavior : MonoBehaviour
{
    #region ===Ride Control===
    public Transform FrogOnTop; //Held by the frog on bottom
    public Transform FrogOnBottom; //Held by the frog on top
    public Transform myBackSpot;
    #endregion

    //Methods
    #region =Getters=
    public bool isBeingRidden()
    {
        return FrogOnTop != null;
    }

    public bool isRiding()
    {
        return FrogOnBottom != null;
    }

    public FrogBehavior GetFrogOnTop()
    {
        return FrogOnTop.GetComponent<FrogBehavior>();
    }

    public FrogBehavior GetFrogOnBottom()
    {
        return FrogOnBottom.GetComponent<FrogBehavior>();
    }

    public Transform GetBackspot()
    {
        return myBackSpot;
    }
    #endregion

    #region ===Ride Control===
    public void connectFrog(Transform frogOnTop)
    {
        FrogOnTop = frogOnTop;
        FrogOnTop.GetComponent<FrogBehavior>().FrogOnBottom = transform; //Give Frog on top ourselves so it knows its on a frog
        FrogOnTop.GetComponent<FrogBehavior>().currentBehavior = BehaviorState.Riding;

        FrogOnTop.GetComponent<AnimateFrog>().ResetTriggers();
        FrogOnTop.GetComponent<AnimateFrog>().LandPickup();
        FrogOnTop.GetComponent<AnimateFrog>().Idle();
    }

    public void disconnectFrogFromBottom()
    {
        if (FrogOnTop != null)
        {
            FrogOnTop.GetComponent<FrogBehavior>().FrogOnBottom = null;
            FrogOnTop = null;
        }
    }

    public void disconnectFrogFromTop()
    {
        if (FrogOnBottom != null)
        {
            FrogOnBottom.GetComponent<FrogBehavior>().FrogOnTop = null;
            FrogOnBottom = null;
        }
    }

    public void jumpOffFrogFromBottom()
    {
        FrogOnTop.GetComponent<FrogBehavior>().JumpToRandomPad();
        disconnectFrogFromBottom();
    }

    public void jumpOffFrogFromTop()
    {
        GetComponent<FrogBehavior>().JumpToRandomPad();
        disconnectFrogFromTop();
    }

    //TO DO: Ienumerator jump off from impatience
    #endregion
}
