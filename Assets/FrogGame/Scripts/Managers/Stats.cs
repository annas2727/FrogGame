using UnityEngine;

[CreateAssetMenu(fileName = "NewFrogStats", menuName = "Frog Stats")]
public class Stats : ScriptableObject
{
    [Header("Lilypad Settings")]
    [Tooltip("Min scale of lilypad that a grown frog can jump on")]
    [SerializeField] private float baseLilypadScale = 0.8f; //Min scale of lilypad that a grown frog can jump on
    [Tooltip("y-offset of frog above lilypad (so that frog doesn't clip into lilypad")]
    [SerializeField] private float baseLilypadyOffset = 0.05f; //y-offset of frog above lilypad (so that frog doesn't clip into lilypad

    [Header("JumpPad Search Settings")]
    [Tooltip("Starting radius frog will search for jump spot")]
    [SerializeField] private float jumpSpotSearchRadius = 10f; //Starting radius frog will search for jump spot
    [Tooltip("How much larger the search radius becomes each search")]
    [SerializeField] private float jumpSpotSearchRadiusAddition = 5f; //How much larger the search radius becomes each search
    [Tooltip("How many times a frog will search for a jump spot till it gives up")]
    [SerializeField] private int jumpSpotSearchNumber = 3; //How many times a frog will search for a jump spot till it gives up
    [Tooltip("Decreased size of search for tilted pads so that jumping off them doesn't look ridiculous")]
    [SerializeField] private float tiltedPadMulti = 0.5f;

    [Header("Jump Control Settings")]
    [Tooltip("Max time frog idles before jumping")]
    [SerializeField] private float maxIdleJumpTime = 10f; //Max time frog idles before jumping
    [Tooltip("Min time frog idles before jumping")]
    [SerializeField] private float minIdleJumpTime = 4f; //Min time frog idles before jumping
    [Tooltip("Speed of frog jump")]
    [SerializeField] private float jumpSpeed = 1f;
    [Tooltip("Frog's gravity when jumping")]
    [SerializeField] private float jumpGravity = -20f;

    [Header("Drag Control Settings")]
    [Tooltip("Time when can be picked up again (should be atleast half a sec to not allow double clicks)")]
    [SerializeField] private float dragCooldown = 1f; //Time when can be picked up again (should be atleast half a sec to not allow double clicks)
    [Tooltip("Max time dragging a frog can be locked to perform some action (Like Breeding)")]
    [SerializeField] private float dragLockMaxTime = 8f;

    [Header("Swim Control Settings")]
    [Tooltip("Swim speed")]
    [SerializeField] private float swimSpeed = 2f;

    [Header("Breeding Settings")]
    [Tooltip("Time before frog can breed again")]
    [SerializeField] private float breedCooldown = 1f;
    [Tooltip("Time before frog jumps off breeding spot due to no partner")]
    [SerializeField] private float breedAnnoyance = 8f;
    [Tooltip("Time after frog gets partner and can still be dragged out to cancel")]
    [SerializeField] private float breedChickenOut = 1.5f;

    [Header("Frog Scale and Growth Settings")]
    [Tooltip("Max scale of frog (when the frog is an adult")]
    [SerializeField] private float maxFrogScale = 0.0025f; //Max scale of frog
    [Tooltip("Min scale of frog (when the frog is a baby)")]
    [SerializeField] private float minFrogScale = 0.0010f; //Min scale of frog
    [Tooltip("Time it takes for frog to go from baby to adult")]
    [SerializeField] private float frogGrowthTime = 300f;

    [SerializeField] private float maxTadpoleScale = 1.1f; //Max scale of tadpole
    [SerializeField] private float minTadpoleScale = 0.8f; //Min scale of tadpole
    [SerializeField] private float tadpoleGrowthTime = 140f;

    //Unknown
    [SerializeField] private float maxEggScale = 0.25f; //Max scale of egg
    [SerializeField] private float minEggScale = 0.20f; //Min scale of egg
    [SerializeField] private float eggGrowthTime = 60f;

    //Lilypad Settings
    public float BaseLilypadScale => baseLilypadScale;
    public float BaseLilypadyOffset => baseLilypadyOffset;

    //Search Settings
    public float JumpSpotSearchRadius => jumpSpotSearchRadius;
    public float JumpSpotSearchRadiusAddition => jumpSpotSearchRadiusAddition;
    public int JumpSpotSearchNumber => jumpSpotSearchNumber;
    public float TiltedPadMulti => tiltedPadMulti;

    //Jump Control Settings
    public float MaxIdleJumpTime => maxIdleJumpTime;
    public float MinIdleJumpTime => minIdleJumpTime;
    public float JumpSpeed => jumpSpeed;
    public float JumpGravity => jumpGravity;

    //Drag Control Settings
    public float DragCooldown => dragCooldown;
    public float DragLockMaxTime => dragLockMaxTime;

    //Swim Control Settings
    public float SwimSpeed => swimSpeed;
    
    //Scale and Growth Settings
    public float MaxFrogScale => maxFrogScale;
    public float MinFrogScale => minFrogScale;
    public float FrogGrowthTime => frogGrowthTime;
    
    public float MaxTadpoleScale => maxTadpoleScale;
    public float MinTadpoleScale => minTadpoleScale;
    public float TadpoleGrowthTime => tadpoleGrowthTime;

    public float MaxEggScale => maxEggScale;
    public float MinEggScale => minEggScale;
    public float EggGrowthTime => eggGrowthTime;

    //Breeding Settings
    public float BreedCooldown => breedCooldown;
    public float BreedAnnoyance => breedAnnoyance;
    public float BreedChickenOut => breedChickenOut;
}