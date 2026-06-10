using UnityEngine;

public class EggColor : MonoBehaviour
{
    MeshRenderer meshRenderer;
    FrogLife frogLife;
    
    GameManager gameManager; 

    void Start()
    {   
        meshRenderer = GetComponent<MeshRenderer>();
        frogLife = GetComponentInParent<FrogLife>();
        gameManager = FindAnyObjectByType<GameManager>();

        ChangeColor();
    }

    void ChangeColor()
    {
        Debug.Log(frogLife.bodyColorName);
        if (ColorUtility.TryParseHtmlString(gameManager.bodyColors[frogLife.bodyColorName], out Color color))
        {
            meshRenderer.materials[0].color = color;
        }
    }
}
