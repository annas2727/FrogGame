using UnityEngine;

public class OpenItem : MonoBehaviour
{
    public GameObject cameraItem;

    void OnMouseDown()
    {
        cameraItem.SetActive(true);
    }
}
