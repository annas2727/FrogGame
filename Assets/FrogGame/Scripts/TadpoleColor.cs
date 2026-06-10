using UnityEngine;

public class TadpoleColor : MonoBehaviour
{
    private SkinnedMeshRenderer skinnedMeshRenderer;

    GameManager gameManager; 
    FrogLife frogLife;

    void Start()
    {
        frogLife = GetComponentInParent<FrogLife>();
        gameManager = FindAnyObjectByType<GameManager>();
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
    }
    
    void ChangeColor()
    {
        if (ColorUtility.TryParseHtmlString(gameManager.tadpoleBodyColors[frogLife.bodyColor], out Color color))
        {
            skinnedMeshRenderer.material.color = color;
        }
    }
}
