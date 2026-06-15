using UnityEngine;

public class EggColor : MonoBehaviour
{
    [SerializeField] private Stats statsConfig;

    MeshRenderer meshRenderer;
    FrogLife frogLife;
    

    void Start()
    {   
        meshRenderer = GetComponent<MeshRenderer>();
        frogLife = GetComponentInParent<FrogLife>();

        ChangeColor();
    }

    void ChangeColor()
    {
        if (ColorUtility.TryParseHtmlString(statsConfig.bodyColors[frogLife.bodyColorName], out Color color))
        {
            meshRenderer.materials[0].color = color;
        }
    }
}
