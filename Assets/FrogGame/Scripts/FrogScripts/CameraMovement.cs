using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{

    [Header("Camera Movement Settings")]
    public float moveSpeed = 5f; 
    public float rotationSpeed = 100f;
    public float mouseSensitivity = 100f;
    public float scrollSensitivity = 10f;

    void Update () 
    {
        HandleMovement();
        HandleScroll();
        HandleRotation();
    }

    void HandleMovement()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        Vector3 direction = Vector3.zero;

        if (keyboard.wKey.isPressed) direction += Vector3.up;
        if (keyboard.sKey.isPressed) direction += Vector3.down;
        if (keyboard.aKey.isPressed) direction += Vector3.left;
        if (keyboard.dKey.isPressed) direction += Vector3.right;
        

        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    void HandleScroll()
    {
            Mouse mouse = Mouse.current;
            if (mouse == null) return;
    
            float scrollValue = mouse.scroll.ReadValue().y;
            transform.Translate(Vector3.forward * scrollValue * scrollSensitivity * Time.deltaTime);
    }

    void HandleRotation()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null) return;

        if (mouse.rightButton.isPressed)
        {
                Vector2 mouseDelta = mouse.delta.ReadValue();
                float rotationX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
                float rotationY = mouseDelta.y * mouseSensitivity * Time.deltaTime;
    
                transform.Rotate(Vector3.up, rotationX);
                transform.Rotate(Vector3.right, -rotationY);
        }
    }
}
