using UnityEngine;

[CreateAssetMenu(fileName = "NewFrogStats", menuName = "Frog Stats")]
public class Stats : ScriptableObject
{
    [Header("Lilypad Settings")]
    [SerializeField] private float baseLilypadScale = 0.8f; //Min scale of lilypad that a grown frog can jump on
    [SerializeField] private float baseLilypadyOffset = 0.05f; //y-offset of frog above lilypad (so that frog doesn't clip into lilypad

    [Header("Search Settings")]
    [SerializeField] private float jumpSpotSearchRadius = 10f; //Starting radius frog will search for jump spot
    [SerializeField] private float jumpSpotSearchRadiusAddition = 5f; //How much larger the search radius becomes each search
    [SerializeField] private int jumpSpotSearchNumber = 3; //How many times a frog will search for a jump spot till it gives up

    [Header("Jump Timing Settings")]
    [SerializeField] private float maxIdleJumpTime = 10f; //Max time frog idles before jumping
    [SerializeField] private float minIdleJumpTime = 4f; //Min time frog idles before jumping

    [Header("Frog Scale Settings")]
    [SerializeField] private float maxFrogScale = 0.25f; //Max scale of frog
    [SerializeField] private float minFrogScale = 0.1f; //Min scale of frog

    [SerializeField] private float maxTadpoleScale = 20f; //Max scale of tadpole
    [SerializeField] private float minTadpoleScale = 10f; //Min scale of tadpole

    //Unknown
    [SerializeField] private float maxEggScale = 0.25f; //Max scale of egg
    [SerializeField] private float minEggScale = 0.1f; //Min scale of egg

    public float BaseLilypadScale => baseLilypadScale;
    public float BaseLilypadyOffset => baseLilypadyOffset;

    public float JumpSpotSearchRadius => jumpSpotSearchRadius;
    public float JumpSpotSearchRadiusAddition => jumpSpotSearchRadiusAddition;
    public int JumpSpotSearchNumber => jumpSpotSearchNumber;

    public float MaxIdleJumpTime => maxIdleJumpTime;
    public float MinIdleJumpTime => minIdleJumpTime;

    public float MaxFrogScale => maxFrogScale;
    public float MinFrogScale => minFrogScale;

    public float MaxTadpoleScale => maxTadpoleScale;
    public float MinTadpoleScale => minTadpoleScale;

    public float MaxEggScale => maxEggScale;
    public float MinEggScale => minEggScale;
}