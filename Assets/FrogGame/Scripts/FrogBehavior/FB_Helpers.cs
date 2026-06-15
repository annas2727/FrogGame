using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class FrogBehavior : MonoBehaviour
{
    public void ChangeBehavior(string behavior)
    {
        if (Enum.TryParse(behavior, true, out BehaviorState result))
            currentBehavior = result;
        else
            currentBehavior = BehaviorState.CanJump;
    }
    public void ChangeBehavior(BehaviorState behavior)
    {
        currentBehavior = behavior;
    }

    //Methods


}
