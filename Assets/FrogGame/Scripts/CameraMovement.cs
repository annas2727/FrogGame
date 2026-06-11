using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{

    [Header("Camera Movement Settings")]
    public float moveSpeed = 5f; 
    public float rotationSpeed = 50f;
    public float mouseSensitivity = 100f;
    public float scrollSensitivity = 20f;

    private float horizontalAxis;
    private float verticalAxis;

    public bool frogPadOpen = false;

    void Start()
    {
        // initialise from whatever rotation the camera starts at
        horizontalAxis = transform.eulerAngles.y;
        verticalAxis = transform.eulerAngles.x;
    }
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

        if (!frogPadOpen)
        {
            if (keyboard.wKey.isPressed) direction += Vector3.forward;
            if (keyboard.sKey.isPressed) direction -= Vector3.forward;
            if (keyboard.aKey.isPressed) direction += Vector3.left;
            if (keyboard.dKey.isPressed) direction += Vector3.right;
            if (keyboard.spaceKey.isPressed) direction += Vector3.up;
            if (keyboard.leftShiftKey.isPressed) direction -= Vector3.up;

        }
        
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    void HandleScroll()
    {
            Mouse mouse = Mouse.current;
            if (mouse == null) return;
    
            float scrollValue = mouse.scroll.ReadValue().y;
            
            if (!frogPadOpen)
            {
                transform.Translate(Vector3.forward * scrollValue * scrollSensitivity * Time.deltaTime);
            }
    }

    void HandleRotation()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null) return;

        if (mouse.rightButton.isPressed)
        {
            Vector2 mouseDelta = mouse.delta.ReadValue();

            horizontalAxis   += mouseDelta.x * mouseSensitivity * Time.deltaTime;
            verticalAxis -= mouseDelta.y * mouseSensitivity * Time.deltaTime;  
            verticalAxis  = Mathf.Clamp(verticalAxis, -80f, 80f);                 

            transform.rotation = Quaternion.Euler(verticalAxis, horizontalAxis, 0f);
        }
    }
}
