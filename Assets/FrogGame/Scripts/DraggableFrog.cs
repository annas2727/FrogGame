using UnityEngine;

public class DraggableFrog : MonoBehaviour
{
    [Tooltip("Optional movement script to disable while dragging")]

    public bool IsBeingDragged { get; private set; }
    public bool HasBeenDragged = false;

    public void BeginDrag()
    {
        IsBeingDragged = true;
        HasBeenDragged = true;
        GetComponent<ParabolicJump>().isJumping = false;

        Vector3 euler = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, euler.y, 0f);
    }

    public void EndDrag()
    {
        IsBeingDragged = false;
    }
}