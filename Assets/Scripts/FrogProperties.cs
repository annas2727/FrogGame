using UnityEngine;

public class FrogProperties : MonoBehaviour
{
    public enum bodyColorName { red, orange, yellow, green, blue, purple }
    public enum patternColorName { red, orange, yellow, green, blue, purple }
    public enum pattern { spots, stripes, none }

    [SerializeField] bodyColorName currentBodyColorName;
    [SerializeField] patternColorName currentPatternColorName;
    [SerializeField] pattern currentPatternType;

    private string bodyColor; 
    private string patternColor;
    private string patternType;

    private string bodyColorName; 
    private string patternColorName;

    public Material frogMaterial;

    private Renderer frogRenderer;
    GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        bodyColorName = currentBodyColorName.ToString();
        patternColorName = currentPatternColorName.ToString();
        patternType = currentPatternType.ToString();

        bodyColor = gameManager.colors.ContainsKey(bodyColorName) ? gameManager.colors[bodyColorName] : "#ffffff";
        patternColor = gameManager.colors.ContainsKey(patternColorName) ? gameManager.colors[patternColorName] : "#ffffff";
        patternType = gameManager.patternTypes.Contains(patternType) ? patternType : gameManager.patternTypes[0];

        frogRenderer = GetComponent<Renderer>();

        if (frogMaterial != null)
            frogRenderer.material = frogMaterial;
        else
            frogMaterial = frogRenderer.material;
            
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
