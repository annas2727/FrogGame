using UnityEngine;

public class FrogFrogPad : MonoBehaviour
{
    Camera frogCam;
    public RenderTexture renderTexture;

    void Awake()
    {
        renderTexture = new RenderTexture(256, 256, 16);
        renderTexture.name = "FrogRT_" + gameObject.name;
        renderTexture.filterMode = FilterMode.Bilinear;
        renderTexture.format = RenderTextureFormat.ARGB32;
        renderTexture.Create();

        frogCam = transform.Find("Camera").GetComponent<Camera>();
        frogCam.targetTexture = renderTexture;
        frogCam.enabled = true;
        
        Debug.Log("Assigned " + renderTexture.name + " to " + frogCam.name);
    }
}