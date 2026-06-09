using UnityEngine;

[ExecuteAlways]
public class FrogLife : MonoBehaviour
{
    public int lifeStage = 1; 
    
    public enum BodyColorOption { red, orange, yellow, green, blue, purple, brown, white, black }
    public enum PatternColorOption { red, orange, yellow, green, blue, purple, brown, white, black }
    public enum PatternType { spots, stripes, none }

    [SerializeField] BodyColorOption currentBodyColor;
    [SerializeField] PatternColorOption currentPatternColor;
    [SerializeField] PatternType currentPatternType;

    public string bodyColorName; 
    public string patternColorName;
    public string patternType;

    public string bodyColor;
    public string patternColor;
    
    GameManager gameManager;

    GameObject egg;
    GameObject tadpole;
    GameObject frog;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        egg = GetChildWithTag("Egg");
        tadpole = GetChildWithTag("Tadpole");
        frog = GetChildWithTag("Frog");

        ChangeLifeStage(1);

        gameManager = FindAnyObjectByType<GameManager>();

        bodyColorName = currentBodyColor.ToString();
        patternColorName = currentPatternColor.ToString();
        patternType = currentPatternType.ToString();

        bodyColor = gameManager.bodyColors.ContainsKey(bodyColorName) ? gameManager.bodyColors[bodyColorName] : "#ffffff";
        patternColor = gameManager.patternColors.ContainsKey(patternColorName) ? gameManager.patternColors[patternColorName] : "#ffffff";
    
    }

    public void ChangeLifeStage(int x)
    {
        if (lifeStage == 1)
        {
            egg.SetActive(true);
            tadpole.SetActive(false);
            frog.SetActive(false);
        } 
        else if (lifeStage == 2)
        {
            egg.SetActive(false);
            tadpole.SetActive(true);
            frog.SetActive(false);
        } 
        else if (lifeStage == 3)
        {
            egg.SetActive(false);
            tadpole.SetActive(false);
            frog.SetActive(true);
        }
    }

    GameObject GetChildWithTag(string tag)
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }
        }
        return null; // Not found
    }

    
}
