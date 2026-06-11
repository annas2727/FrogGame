using UnityEngine;

public class FrogTop : MonoBehaviour
{
    [SerializeField] private Stats statsConfig;

    public Transform FrogOnTop; //Held by the frog on bottom
    public Transform FrogOnBottom; //Held by the frog on top
    public Transform backSpot; //Target on frog on bottom
    void Start()
    {
        backSpot = transform.Find("BackSpot");
    }

    void Update()
    {
        if (FrogOnTop != null)
        {
            FrogOnTop.position = backSpot.position;
            FrogOnTop.rotation = transform.rotation * Quaternion.Euler(-8f, 0f, 0f); ;
        }
    }

    public void connectFrog(Transform frog)
    {
        FrogOnTop = frog;
        FrogOnTop.GetComponent<FrogTop>().FrogOnBottom = transform; //Give Frog on top ourselves so it knows its on a frog

        FrogOnTop.GetComponent<AnimateFrog>().ResetTriggers();
        frog.GetComponent<AnimateFrog>().LandPickup();
        frog.GetComponent<AnimateFrog>().Idle();
        
        //FrogOnTop.GetComponent<AnimateFrog>().Idle();
    }



    public void disconnectFrogFromBottom() //Disconnect Frog From adult frog
    {
        if (FrogOnTop != null)
        {
            FrogOnTop.GetComponent<FrogTop>().FrogOnBottom = null;
            FrogOnTop = null;
        }
    }

    public void disconnectFrogFromTop()
    {
        if (FrogOnBottom != null)
        { 
            FrogOnBottom.GetComponent<FrogTop>().FrogOnTop = null;
            FrogOnBottom = null;
        }
    }



    public void jumpOffFrogFromBottom()
    {
        FrogOnTop.GetComponent<FrogChooseJump>().JumpToNearbyPad();
        disconnectFrogFromBottom();
    }

    public void jumpOffFrogFromTop()
    {
        GetComponent<FrogChooseJump>().JumpToNearbyPad();
        disconnectFrogFromTop();
    }
}
