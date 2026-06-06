using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableFrog : MonoBehaviour
{
    [SerializeField] private Stats statsConfig;

    public bool IsBeingDragged { get; private set; }
    public bool HasBeenDragged = false;

    public bool CanBeDragged = true;

    IEnumerator PickupCooldown()
    {
        yield return new WaitForSeconds(statsConfig.PickUpCooldown);
        CanBeDragged = true;
    }

    public void BeginDrag()
    {
        IsBeingDragged = true;
        HasBeenDragged = true; //purely to fix rotation issue when dragging a tilted frog
        CanBeDragged = false; //for the cooldown
        StartCoroutine(PickupCooldown());
        GetComponent<JumpBetweenSpots>().isOccupied = true; //to not activate the jump away script
        GetComponent<ParabolicJump>().isJumping = false; //To catch midair (Stop midair jump)
        GetComponent<FrogSwim>().isSwimming = false; //To catch midswim (Stop midwater swim)

        Vector3 euler = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, euler.y, 0f);

        GetComponent<AnimateFrog>().Pickup();
        
    }

    public void EndDrag()
    {
        IsBeingDragged = false;
    }
}