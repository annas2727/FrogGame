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

    #region ===Ride Control===
    public void connectFrog(Transform frogOnTop)
    {
        FrogOnTop = frogOnTop;
        FrogOnTop.GetComponent<FrogBehavior>().FrogOnBottom = transform; //Give Frog on top ourselves so it knows its on a frog

        FrogOnTop.GetComponent<AnimateFrog>().ResetTriggers();
        FrogOnTop.GetComponent<AnimateFrog>().LandPickup();
        FrogOnTop.GetComponent<AnimateFrog>().Idle();
    }

    public void disconnectFrogFromBottom()
    {
        if (FrogOnTop != null)
        {
            FrogOnTop.GetComponent<FrogTop>().FrogOnBottom = null;
            FrogOnTop = null;
        }
    }

    public void disconnectFrogFromTop()
    {
        if (FrogOnBottom != null)
        {
            FrogOnBottom.GetComponent<FrogTop>().FrogOnTop = null;
            FrogOnBottom = null;
        }
    }

    public void jumpOffFrogFromBottom()
    {
        FrogOnTop.GetComponent<FrogChooseJump>().JumpToNearbyPad();
        disconnectFrogFromBottom();
    }

    public void jumpOffFrogFromTop()
    {
        GetComponent<FrogChooseJump>().JumpToNearbyPad();
        disconnectFrogFromTop();
    }

    //TO DO: Ienumerator jump off from impatience
    #endregion
}
