using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class FrogBehavior : MonoBehaviour
{


    //Methods

    #region === Pad Control ===

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
}
