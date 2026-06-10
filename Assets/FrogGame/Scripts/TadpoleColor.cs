using UnityEngine;

public class TadpoleColor : MonoBehaviour
{
    SkinnedMeshRenderer skinnedMeshRenderer;
    FrogLife frogLife;
    
    GameManager gameManager; 

    void Start()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        frogLife = GetComponentInParent<FrogLife>();
        gameManager = FindAnyObjectByType<GameManager>();

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
