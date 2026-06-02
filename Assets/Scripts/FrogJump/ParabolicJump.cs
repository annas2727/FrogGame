using UnityEngine;

public class ParabolicJump : MonoBehaviour
{
    [SerializeField] private Stats statsConfig;

    public AnimateFrog frog;

    [SerializeField] private float gravity = -20f;

    private Vector3 startPos;
    public Vector3 targetPos;

    private Vector3 horizontalVelocity;
    private float verticalVelocity;

    private float jumpDuration;
    private float elapsedTime;

    private Quaternion startRotation;
    private Quaternion targetRotation;

    public bool isJumping;

    [SerializeField] private float jumpSpeed = 1f;

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

    public void JumpToPad(GameObject pad)
    {
        startRotation = transform.rotation;

        //Vector3 euler = transform.rotation.eulerAngles;
        //Vector3 padEuler = pad.transform.rotation.eulerAngles;

        // Rotate so the frog's up matches the pad's up
        targetRotation = Quaternion.FromToRotation(
            transform.up,
            pad.transform.up
        ) * transform.rotation;

        /*
        targetRotation = Quaternion.Euler(
            padEuler.x,
            euler.y,
            padEuler.z
        );
        */
        JumpTo(
            pad.transform.position + Vector3.up * statsConfig.BaseLilypadyOffset,
            jumpSpeed
        );
    }

    private void Update()
    {
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
        float rotationT = Mathf.Clamp01(elapsedTime / jumpDuration);

        transform.rotation = Quaternion.Slerp(
            startRotation,
            targetRotation,
            rotationT
        );

        if (elapsedTime >= jumpDuration)
        {
            transform.position = targetPos;
            transform.rotation = targetRotation;

            isJumping = false;
            frog.SetAnimationState("Idle");
        }
    }
}