using UnityEngine;

public class FrogImageRecolor : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public Color bodyColor = Color.red;
    public Color patternColor = Color.blue;
    [Range(0.1f, 1f)]
    public float threshold = 0.5f;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        Apply();
    }

    public void Apply()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        meshRenderer.GetPropertyBlock(block);
        block.SetColor("_BodyColor", bodyColor);
        block.SetColor("_PatternColor", patternColor);
        block.SetFloat("_Threshold", threshold);
        meshRenderer.SetPropertyBlock(block);
    }
}