using UnityEngine;

public class FrogTop : MonoBehaviour
{
    private Transform FrogOnTop;
    public Transform backSpot;
    void Start()
    {
        backSpot = transform.Find("BackSpot");
    }

    void Update()
    {
        if (FrogOnTop != null)
        {
            FrogOnTop.position = backSpot.position;
            FrogOnTop.rotation = transform.rotation;
        }
    }

    public void connectFrog(Transform frog)
    {
        FrogOnTop = frog;
        FrogOnTop.GetComponent<AnimateFrog>().Idle();
    }

    public void disconnectFrog()
    {
        FrogOnTop = null;
    }

    public void jumpOffFrog()
    {
        FrogOnTop.GetComponent<FrogChooseJump>().JumpToNearbyPad();
        disconnectFrog();
    }
}
