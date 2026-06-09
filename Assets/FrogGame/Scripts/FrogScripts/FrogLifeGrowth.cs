using UnityEngine;

public class FrogLifeGrowth : MonoBehaviour
{
    [SerializeField] private Stats statsConfig;

    
    public int LifeStage = 0; //0=egg, 1=tadpole, 2=froglet, 3=frog
    private float ageInStage; //age in that lifestage
    private float growthTime; //Time to fully grow
    private float maxScale;
    private float minScale;
    void Start()
    {
        UpdateLifeStage(0);
    }

    void Update()
    {
        ageInStage += Time.deltaTime;
        float t = Mathf.Clamp01(ageInStage / growthTime);

        float scale = Mathf.Lerp(minScale, maxScale, ageInStage / growthTime);
        transform.localScale = Vector3.one * scale;

        if (ageInStage > growthTime)
            UpdateLifeStage(LifeStage + 1);
    }

    private void UpdateLifeStage(int stage)
    {
        LifeStage = stage;
        switch (stage)
        {
            case 0:
                growthTime = statsConfig.EggGrowthTime;
                maxScale = statsConfig.MaxEggScale;
                minScale = statsConfig.MinEggScale;
                break;
            case 1:
                growthTime = statsConfig.TadpoleGrowthTime;
                maxScale = statsConfig.MaxTadpoleScale;
                minScale = statsConfig.MinTadpoleScale;
                break;
            case 2:
                growthTime = statsConfig.FrogGrowthTime;
                maxScale = statsConfig.MaxFrogScale;
                minScale = statsConfig.MinFrogScale;
                break;
            default:
            LifeStage = 3;
            growthTime = 0f;
            maxScale = statsConfig.MaxFrogScale;
            minScale = statsConfig.MaxFrogScale;
            break;
        }
    }
}
