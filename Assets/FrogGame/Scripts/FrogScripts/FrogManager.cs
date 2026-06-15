using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FrogManager : MonoBehaviour
{
    [SerializeField] private LayerMask frogLayer;
    [SerializeField] private LayerMask floorLayer;

    private Camera mainCamera;

    private FrogBehavior draggedFrog;
    private Vector3 dragOffset;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        #region ===Mouse Control===
        Mouse mouse = Mouse.current;

        if (mouse == null)
            return;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            TryStartDrag(mouse.position.ReadValue());
        }

        if (draggedFrog != null)
        {
            UpdateDrag(mouse.position.ReadValue());
        }

        if (mouse.leftButton.wasReleasedThisFrame)
        {
            EndDrag();
        }
        #endregion
    }

    #region ===Start Dragging===
    private void TryStartDrag(Vector2 mousePosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        //Try hit frog
        if (!Physics.Raycast(ray, out RaycastHit frogHit, 1000f, frogLayer))
            return;

        FrogBehavior frog = frogHit.collider.GetComponentInParent<FrogBehavior>();

        if (frog == null || !frog.CanBeDragged())
            return;
        draggedFrog = frog;

        #region ===Riding Control===
        //Get Frog on top of if there is a frog on top of
        if (frog.isBeingRidden())
        {
            //Set draggedFrog to the frog on top
            draggedFrog = frog.GetFrogOnTop();
            draggedFrog.disconnectFrogFromTop();
        }
        #endregion

        draggedFrog.BeginDrag();

        // Calculate offset so the frog doesn't snap
        if (Physics.Raycast(ray, out RaycastHit floorHit, 1000f, floorLayer))
        {
            dragOffset = draggedFrog.transform.position - floorHit.point;
        }
        else
        {
            dragOffset = Vector3.zero;
        }
    }
    #endregion

    #region ===End Dragging===
    private void UpdateDrag(Vector2 mousePosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit floorHit, 1000f, floorLayer))
            return;

        draggedFrog.transform.position = floorHit.point + dragOffset + Vector3.up * 0.25f;
    }

    private void EndDrag()
    {
        if (draggedFrog == null)
            return;

        draggedFrog.EndDrag();
        StartCoroutine(DropToFloor(draggedFrog.transform));

        draggedFrog = null;
    }
    #endregion

    private FrogBehavior GetFrogUnder(Transform frog)
    {
        Vector3 origin = frog.position + Vector3.up * 5f;
        RaycastHit[] frogsHit = Physics.RaycastAll(origin, Vector3.down, 20f, frogLayer);
        if (frogsHit.Length > 0)
        {
            RaycastHit furthestHit = frogsHit[0];
            foreach (RaycastHit frogHit in frogsHit)
            {
                if (frogHit.distance > furthestHit.distance)
                    furthestHit = frogHit;
            }
            return furthestHit.transform.GetComponent<FrogBehavior>();
        }
        return null;
    }

    private IEnumerator DropToFloor(Transform FallingFrogTransform)
    {
        FrogBehavior FallingFrog = FallingFrogTransform.GetComponent<FrogBehavior>();

        Vector3 start = FallingFrogTransform.position;
        Vector3 origin = FallingFrogTransform.position + Vector3.up * 5f;
        Vector3 target;

        if (!Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 20f, floorLayer))
            yield break;

        //What we are falling onto
        string floorTag = hit.collider.tag;
        target = hit.point + Vector3.down * 0.05f;

        #region ===Riding Control===
        //Hitting a frog
        FrogBehavior FrogOnBottom = GetFrogUnder(FallingFrogTransform);
        if (FrogOnBottom != null)
        {
            Debug.Log("Hit a frog");
            //If we hit an adult and we are holding a child
            if (FrogOnBottom.isAdult && !FallingFrog.isAdult)
            {
                //Check if the spot is empty
                if(!FrogOnBottom.isBeingRidden())
                {
                    //Fall onto frog
                    Debug.Log("Target the frog's back");
                    target = FrogOnBottom.GetBackspot().position;
                    floorTag = "Frog";
                }
            }
        }
        #endregion

        #region ===Breed Control
        //Fall into breedspot
        bool canBreed = false;
        if (floorTag == "BreedingSpot" && FallingFrog.isAdult) {
            BreedSpot targetBreedSpot = hit.collider.transform.GetComponent<BreedSpot>(); //Get the breedspot we hit
            if (targetBreedSpot != null && !targetBreedSpot.isReserved) //If it exists and is not reserved
            { 
                target = targetBreedSpot.transform.position; //Make the frog fall into the breedspot
                FallingFrog.ConnectBreedSpot(targetBreedSpot);
                canBreed = true;
            }
        }
        #endregion

        #region ===Falling Math===
        float t = 0f;
        float duration = 0.25f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            FallingFrogTransform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        FallingFrogTransform.position = target;
        #endregion

        #region ===Falling Onto===
        // Handle special floor types
        AnimateFrog FrogAnimation = FallingFrogTransform.GetComponent<AnimateFrog>();
        switch (floorTag)
        {
            case "Water":
                Debug.Log("Frog entered Water");
                //Swimming doesn't have land animation
                FallingFrog.StartSwimming();
                break;

            case "BreedingSpot":
                Debug.Log("Frog entered BreedingSpot");
                FrogAnimation.LandPickup();
                if (canBreed)
                    FallingFrog.StartTryBreeding();
                else
                    FallingFrog.JumpToRandomPad(); //Jump Away Quickly
                break;

            case "Frog":
                Debug.Log("Frog land on frog yay");
                FrogAnimation.LandPickup();
                FrogOnBottom.connectFrog(FallingFrogTransform);
                break;

            default:
                Debug.Log("Landed");
                FrogAnimation.LandPickup();
                FallingFrog.ChangeBehavior("CanJump"); //Let them get back to jumping
                break;
        }
        #endregion
    }
}