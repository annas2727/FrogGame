using UnityEngine;

public class ClosePad : MonoBehaviour
{
    public GameObject cameraItem;
    public CameraMovement mainCamera;

    void OnMouseDown()
    {
        cameraItem.SetActive(false);
        mainCamera.frogPadOpen = false; 
    }
}
