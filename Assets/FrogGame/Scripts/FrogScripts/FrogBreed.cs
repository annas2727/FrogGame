using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBreed : MonoBehaviour
{
    [SerializeField] private Stats statsConfig;

    public bool IsBeingDragged { get; private set; }
    public bool HasBeenDragged = false;

    public bool CanBeDragged = true;

    IEnumerator BreedCooldown()
    {
        yield return new WaitForSeconds(statsConfig.BreedCooldown);
        CanBeDragged = true;
    }
}