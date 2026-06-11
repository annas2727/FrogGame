using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragFrogManager : MonoBehaviour
{
    [SerializeField] private LayerMask frogLayer;
    [SerializeField] private LayerMask floorLayer;

    private Camera mainCamera;

    private FrogDrag draggedFrog;
    private Vector3 dragOffset;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
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
    }

    private void TryStartDrag(Vector2 mousePosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit frogHit, 1000f, frogLayer))
            return;


        FrogDrag frog = frogHit.collider.GetComponentInParent<FrogDrag>();

        if (frog == null || !frog.CanBeDragged)
            return;
        draggedFrog = frog;

        //Get Frog on top of if there is a frog on top of
        if (frog.gameObject.GetComponent<FrogTop>().FrogOnTop != null)
        {
            //Set draggedFrog to the frog on top
            Transform FrogOnTop = frog.gameObject.GetComponent<FrogTop>().FrogOnTop;
            draggedFrog = FrogOnTop.GetComponent<FrogDrag>();
            frog.gameObject.GetComponent<FrogTop>().disconnectFrogFromTop();
        }

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

    private IEnumerator DropToFloor(Transform frog)
    {
        Vector3 start = frog.position;
        Vector3 origin = frog.position + Vector3.up * 5f;
        Vector3 target;

        if (!Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 20f, floorLayer))
            yield break;

        string floorTag = hit.collider.tag;
        target = hit.point + Vector3.down * 0.05f;

        //Hitting a frog
        Transform hitFrog = null;
        RaycastHit[] frogsHit = Physics.RaycastAll(origin, Vector3.down, 20f, frogLayer);
        if (frogsHit.Length > 0)
        {
            RaycastHit furthestHit = frogsHit[0];

            foreach (RaycastHit frogHitinfrogsHit in frogsHit)
            {
                if (frogHitinfrogsHit.distance > furthestHit.distance)
                {
                    furthestHit = frogHitinfrogsHit;
                }
            }
            hitFrog = furthestHit.transform;

            Debug.Log("Hit a frog");
            //If we hit an adult and we are holding a child
            if (hitFrog.GetComponent<FrogBreed>().isAdult && !frog.GetComponent<FrogBreed>().isAdult)
            {
                //Check if the spot is empty
                if(hitFrog.GetComponent<FrogTop>().FrogOnTop == null)
                {
                    //Fall onto frog
                    Debug.Log("Target the frog's back");
                    target = hitFrog.GetComponent<FrogTop>().backSpot.position;
                    floorTag = "Frog";
                }
            }
            else
            {
                Debug.Log("Frog not adult or dragged frog not kid");
            }
        }

        //Fall into breedspot
        bool canBreed = false;
        if (floorTag == "BreedingSpot" && frog.GetComponent<FrogBreed>().isAdult) {
            BreedSpot targetBreedSpot = hit.collider.transform.GetComponent<BreedSpot>(); //Get the breedspot we hit
            if (targetBreedSpot != null){ //If it exists
                if (!targetBreedSpot.isReserved){ //If it is not reserved
                    target = targetBreedSpot.transform.position; //Make the frog fall into the breedspot
                    frog.GetComponent<FrogBreed>().ConnectBreedSpot(targetBreedSpot);
                    canBreed = true;
                }
            }
        }

        float t = 0f;
        float duration = 0.25f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            frog.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        frog.position = target;

        // Handle special floor types
        switch (floorTag)
        {
            case "Water":
                Debug.Log("Frog entered water");
                frog.GetComponent<FrogSwim>().StartSwimming(); //Start swimming in water
                break;

            case "BreedingSpot":
                Debug.Log("Frog entered breeding spot");
                frog.GetComponent<AnimateFrog>().LandPickup(); //Play land animation
                if (canBreed)
                    frog.GetComponent<FrogBreed>().StartTryBreeding();
                else
                    frog.GetComponent<FrogChooseJump>().isOccupied = false; //Let them get back to jumping
                break;

            case "Frog": //When landing on a frog
                Debug.Log("Frog land on frog yay");
                frog.GetComponent<AnimateFrog>().LandPickup();
                hitFrog.GetComponent<FrogTop>().connectFrog(frog);
                break;

            default:
                frog.GetComponent<FrogChooseJump>().isOccupied = false; //Let them get back to jumping
                frog.GetComponent<AnimateFrog>().LandPickup(); //Play land animation
                break;
        }
    }
}