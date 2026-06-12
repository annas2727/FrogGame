using UnityEngine;

public class MoveFrogCam : MonoBehaviour
{
    Camera frogCam; 
    FrogLife frogLife; 
    Vector3 camLocalOffset;

    void Start()
    {
        frogCam = GetComponentInChildren<Camera>();
        frogLife = GetComponent<FrogLife>();
        camLocalOffset = frogCam.transform.localPosition;
    }

    void MoveCamera()
    {
        frogCam.transform.position = frogLife.activated.transform.position + camLocalOffset;
        /*
        if (frogLife.LifeStage == 0)
        {
            frogCam.position = frogCam.localPostion + frogLife.egg.position; 
        }
        else if (frogLife.LifeStage == 1)
        {
            frogCam.position = frogCam.localPostion + frogLife.egg.position; 
        } 
        else if (frogLife.LifeStage == 2)
        {
            frogCam.position = frogCam.localPostion + frogLife.frog.position; 
        }*/
    }

    void Update()
    {
        MoveCamera(); 
    }
}
