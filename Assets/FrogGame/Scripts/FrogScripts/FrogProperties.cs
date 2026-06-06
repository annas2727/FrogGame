using UnityEngine;

public class FrogProperties : MonoBehaviour
{
    public enum BodyColorOption { red, orange, yellow, green, blue, purple, brown, white, black }
    public enum PatternColorOption { red, orange, yellow, green, blue, purple, brown, white, black }
    public enum PatternType { spots, stripes, none }

    [SerializeField] BodyColorOption currentBodyColor;
    [SerializeField] PatternColorOption currentPatternColor;
    [SerializeField] PatternType currentPatternType;

    private string bodyColor;
    private string patternColor;
    private string patternType;
    private Material frogMaterial;

    private Renderer frogRenderer;
    private SkinnedMeshRenderer frogSkinnedRenderer;
    GameManager gameManager;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();

        string bodyColorName = currentBodyColor.ToString();
        string patternColorName = currentPatternColor.ToString();
        patternType = currentPatternType.ToString();

        bodyColor = gameManager.bodyColors.ContainsKey(bodyColorName) ? gameManager.bodyColors[bodyColorName] : "#ffffff";
        patternColor = gameManager.patternColors.ContainsKey(patternColorName) ? gameManager.patternColors[patternColorName] : "#ffffff";
    
        frogRenderer = GetComponent<Renderer>();
        frogSkinnedRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    
        Material sharedMaterial = gameManager.GetPatternMaterial(patternType);
        if (sharedMaterial != null)
        {
            frogMaterial = new Material(sharedMaterial);
        }
        else
        {
            Debug.LogWarning($"No material found for pattern type: {patternType}. Using default material.");
            frogMaterial = new Material(Shader.Find("Standard"));
        }

        frogRenderer.material = frogMaterial;

        //Replace the original skin with the one that has the pattern shader
        Material[] mats = frogSkinnedRenderer.materials;
        mats[1] = gameManager.eyeMaterial;
        mats[0] = frogMaterial; 
        frogSkinnedRenderer.materials = mats;

        ApplyProperties();
    }
    
    public void ApplyProperties()
    {
        if (frogMaterial == null) return;

        SetColor("_Body", bodyColor);
        SetColor("_Spots", patternColor);
        SetColor("_Stripes", patternColor); 
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