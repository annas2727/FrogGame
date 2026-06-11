using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class FrogBehavior : MonoBehaviour
{
    #region === Frog Drag ===

    public bool InDragCooldown = true;

    IEnumerator StartDragCooldown()
    {
        yield return new WaitForSeconds(statsConfig.DragCooldown);
        InDragCooldown = false;
    }

    public void BeginDrag()
    {
        StartCoroutine(StartDragCooldown()); //Start DragCooldown

        currentBehavior = BehaviorState.Dragging;

        //GetComponent<FrogBreed>().ChangeBreedPhase(0); //Cancel Breeding
        //GetComponent<FrogTop>().disconnectFrogFromTop();

        Vector3 euler = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, euler.y, 0f);

        GetComponent<AnimateFrog>().ResetTriggers();
        GetComponent<AnimateFrog>().Pickup();
        Debug.Log("In frog drag mode");
    }

    public void EndDrag()
    {
        GetComponent<AnimateFrog>().ResetTriggers();
    }

    #endregion
}
