using UnityEngine;

public class OpenItem : MonoBehaviour
{
    public GameObject cameraItem;
    public CameraMovement mainCamera;

    void Start()
    {
        mainCamera.frogPadOpen = true; 
    }

    void OnMouseDown()
    {
        cameraItem.SetActive(true);
    }
}
