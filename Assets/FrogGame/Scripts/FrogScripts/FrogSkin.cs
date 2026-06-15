using UnityEngine;

public class FrogSkin : MonoBehaviour
{
    [SerializeField] private Stats statsConfig;

    private Material frogMaterial;

    private Renderer frogRenderer;
    private SkinnedMeshRenderer frogSkinnedRenderer;

    private FrogLife frogLife; 

    void Start()
    {
        frogLife = GetComponentInParent<FrogLife>();

        frogRenderer = GetComponent<Renderer>();
        frogSkinnedRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    
        Material sharedMaterial = statsConfig.GetPatternMaterial(frogLife.patternType);
        if (sharedMaterial != null)
        {
            frogMaterial = new Material(sharedMaterial);
        }
        else
        {
            frogMaterial = new Material(Shader.Find("Standard"));
        }

        frogRenderer.material = frogMaterial;

        //Replace the original skin with the one that has the pattern shader
        Material[] mats = frogSkinnedRenderer.materials;
        mats[1] = statsConfig.eyeMaterial;
        mats[0] = frogMaterial; 
        frogSkinnedRenderer.materials = mats;

        ApplyProperties();
    }
    
    public void ApplyProperties()
    {
        if (frogMaterial == null) return;

        SetColor("_Body", frogLife.bodyColor);
        SetColor("_Spots", frogLife.patternColor);
        SetColor("_Stripes", frogLife.patternColor); 
        //SetPatternType(patternType);
    }

    private void SetColor(string propertyName, string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
            frogMaterial.SetColor(propertyName, color);
        else
            Debug.LogWarning($"Invalid hex color: {hex}");
    }
}