using UnityEngine;

public class FrogLife : MonoBehaviour
{
    [SerializeField] private Stats statsConfig;

    public int LifeStage = 0; //0=egg, 1=tadpole, 2=froglet, 3=frog
    private float ageInStage = 0f; //age in that lifestage
    private float growthTime = 0f; //Time to fully grow
    private float maxScale;
    private float minScale;
    public bool skipLifeStage = false; //For testing
    
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
    GameObject activated;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        egg = GetChildWithTag("Egg");
        tadpole = GetChildWithTag("Tadpole");
        frog = GetChildWithTag("Frog");

        gameManager = FindAnyObjectByType<GameManager>();

        bodyColorName = currentBodyColor.ToString();
        patternColorName = currentPatternColor.ToString();
        patternType = currentPatternType.ToString();

        bodyColor = gameManager.bodyColors.ContainsKey(bodyColorName) ? gameManager.bodyColors[bodyColorName] : "#ffffff";
        patternColor = gameManager.patternColors.ContainsKey(patternColorName) ? gameManager.patternColors[patternColorName] : "#ffffff";

        UpdateLifeStage(0);
    }

    private void Update()
    {
        if (LifeStage < 3)
        {
            ageInStage += Time.deltaTime;
            float t = Mathf.Clamp01(ageInStage / growthTime);
            float scale = Mathf.Lerp(minScale, maxScale, ageInStage / growthTime);
            activated.transform.localScale = Vector3.one * scale;

            if (ageInStage > growthTime)
                UpdateLifeStage(LifeStage + 1);
            if (skipLifeStage)
            {
                skipLifeStage = false;
                UpdateLifeStage(LifeStage + 1);
            }
        }
    }

    private void UpdateLifeStage(int stage)
    {
        ageInStage = 0f;
        LifeStage = stage;
        switch (stage)
        {
            case 0:
                growthTime = statsConfig.EggGrowthTime;
                maxScale = statsConfig.MaxEggScale;
                minScale = statsConfig.MinEggScale;
                egg.SetActive(true);
                tadpole.SetActive(false);
                frog.SetActive(false);
                activated = egg;
                break;
            case 1:
                growthTime = statsConfig.TadpoleGrowthTime;
                maxScale = statsConfig.MaxTadpoleScale;
                minScale = statsConfig.MinTadpoleScale;
                egg.SetActive(false);
                tadpole.SetActive(true);
                frog.SetActive(false);
                activated = tadpole;
                break;
            case 2:
                growthTime = statsConfig.FrogGrowthTime;
                maxScale = statsConfig.MaxFrogScale;
                minScale = statsConfig.MinFrogScale;
                egg.SetActive(false);
                tadpole.SetActive(false);
                frog.SetActive(true);
                activated = frog;
                break;
            default:
                LifeStage = 3;
                growthTime = 0f;
                maxScale = statsConfig.MaxFrogScale;
                minScale = statsConfig.MaxFrogScale;
                egg.SetActive(false);
                tadpole.SetActive(false);
                frog.SetActive(true);
                activated = frog;
                frog.GetComponent<FrogBreed>().isAdult = true; //Makes the frog an adult
                break;
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
