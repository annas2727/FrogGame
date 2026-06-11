using UnityEngine;

public class FrogFrogPad : MonoBehaviour
{
    Camera frogCam;
    public RenderTexture renderTexture;

    void Awake()
    {
        renderTexture = new RenderTexture(256, 256, 16)
        {
            name = "FrogRT_" + gameObject.name,
            filterMode = FilterMode.Bilinear,
            format = RenderTextureFormat.ARGB32
        };
        renderTexture.Create();

        frogCam = transform.Find("Camera").GetComponent<Camera>();
        frogCam.targetTexture = renderTexture;
        frogCam.enabled = true;
    }
}