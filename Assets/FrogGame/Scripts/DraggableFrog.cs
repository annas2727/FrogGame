using UnityEngine;

public class DraggableFrog : MonoBehaviour
{
    [Tooltip("Optional movement script to disable while dragging")]
    public MonoBehaviour movementScript;

    public bool IsBeingDragged { get; private set; }

    public void BeginDrag()
    {
        IsBeingDragged = true;

        if (movementScript != null)
            movementScript.enabled = false;
    }

    public void EndDrag()
    {
        IsBeingDragged = false;

        if (movementScript != null)
            movementScript.enabled = true;
    }
}