using UnityEngine;

[CreateAssetMenu(fileName = "NewFrogStats", menuName = "Frog Stats")]
public class Stats : ScriptableObject
{
    [SerializeField] private float baseLilypadScale = 0.8f; //Min scale of lilypad that a grown frog can jump on
    [SerializeField] private float baseLilypadyOffset = 0.05f; //y-offset of frog above lilypad (so that frog doesn't clip into lilypad

    [SerializeField] private float maxFrogScale = 0.25f; //Max scale of frog
    [SerializeField] private float minFrogScale = 0.1f; //Min scale of frog

    [SerializeField] private float maxTadpoleScale = 20f; //Max scale of tadpole
    [SerializeField] private float minTadpoleScale = 10f; //Min scale of tadpole

    //Unknown
    [SerializeField] private float maxEggScale = 0.25f; //Max scale of egg
    [SerializeField] private float minEggScale = 0.1f; //Min scale of egg

    public float BaseLilypadScale => baseLilypadScale;
    public float BaseLilypadyOffset => baseLilypadyOffset;

    public float MaxFrogScale => maxFrogScale;
    public float MinFrogScale => minFrogScale;

    public float MaxTadpoleScale => maxTadpoleScale;
    public float MinTadpoleScale => minTadpoleScale;

    public float MaxEggScale => maxEggScale;
    public float MinEggScale => minEggScale;
}