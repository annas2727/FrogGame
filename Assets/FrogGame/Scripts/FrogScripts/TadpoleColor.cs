using UnityEngine;

public class TadpoleColor : MonoBehaviour
{
    [SerializeField] private Stats statsConfig;

    SkinnedMeshRenderer skinnedMeshRenderer;
    FrogLife frogLife;

    void Start()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        frogLife = GetComponentInParent<FrogLife>();

        ChangeColor();
    }


    void ChangeColor()
    {
        if (ColorUtility.TryParseHtmlString(statsConfig.tadpoleBodyColors[frogLife.bodyColorName], out Color color))
        {
            skinnedMeshRenderer.material.color = color;
        }
    }
}
