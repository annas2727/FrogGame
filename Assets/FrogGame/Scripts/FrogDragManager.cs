using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FrogDragManager : MonoBehaviour
{
    [SerializeField] private LayerMask frogLayer;
    [SerializeField] private LayerMask floorLayer;

    private Camera mainCamera;

    private DraggableFrog draggedFrog;
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

        DraggableFrog frog = frogHit.collider.GetComponentInParent<DraggableFrog>();

        if (frog == null)
            return;

        draggedFrog = frog;
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

        if (!Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 20f, floorLayer))
            yield break;

        Vector3 target = hit.point;

        float t = 0f;
        float duration = 0.15f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            frog.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        frog.position = target;
    }
}