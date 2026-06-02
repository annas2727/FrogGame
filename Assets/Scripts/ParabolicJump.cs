using UnityEngine;

public class ParabolicJump : MonoBehaviour
{
    [SerializeField] private float gravity = -20f;

    private Vector3 startPos;
    public Vector3 targetPos;

    private Vector3 horizontalVelocity;
    private float verticalVelocity;

    private float jumpDuration;
    private float elapsedTime;

    public bool isJumping;

    public bool test = false;
    public AnimateFrog frog; 
    
    public void JumpTo(Vector3 target, float duration)
    {
        frog.SetAnimationState("Jump");
        startPos = transform.position;
        targetPos = target;

        jumpDuration = duration;
        elapsedTime = 0f;

        Vector3 displacement = targetPos - startPos;

        // Horizontal motion (XZ)
        Vector3 horizontalDisplacement = new Vector3(
            displacement.x,
            0f,
            displacement.z
        );

        horizontalVelocity = horizontalDisplacement / jumpDuration;

        // Vertical motion (Y)
        verticalVelocity =
            (displacement.y - 0.5f * gravity * jumpDuration * jumpDuration)
            / jumpDuration;

        isJumping = true;
    }

    public void JumpToPad(GameObject Pad)
    {
        JumpTo(Pad.transform.position + Vector3.up, 2f);
    }

    private void Update()
    {
        if (test)
        {
            Vector3 targetTest = new Vector3(10, 0, 10);
            JumpTo(targetTest, 2f);
            test = false;
        }
        if (!isJumping)
        {
            return;
        }
           

        elapsedTime += Time.deltaTime;

        float t = Mathf.Min(elapsedTime, jumpDuration);

        Vector3 horizontalOffset = horizontalVelocity * t;

        float verticalOffset =
            verticalVelocity * t +
            0.5f * gravity * t * t;

        transform.position =
            startPos +
            horizontalOffset +
            Vector3.up * verticalOffset;

        if (elapsedTime >= jumpDuration)
        {
            transform.position = targetPos;
            isJumping = false;
            frog.SetAnimationState("Idle");
        }
    }
}