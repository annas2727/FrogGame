using UnityEngine;

[ExecuteAlways]
public class Tadpole : MonoBehaviour
{
    public enum BodyColorOption { red, orange, yellow, green, blue, purple, brown, white, black }

    [SerializeField] BodyColorOption currentBodyColor;

    private string bodyColor;
    
    private SkinnedMeshRenderer skinnedMeshRenderer;

    public GameManager gameManager; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
    }
    
    void ChangeColor()
    {
        string bodyColor = currentBodyColor.ToString();

        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        if (ColorUtility.TryParseHtmlString(gameManager.tadpoleBodyColors[bodyColor], out Color color))
        {
            skinnedMeshRenderer.material.color = color;
        }
    }
}
