using UnityEngine;

public class TadpoleSwimmer : MonoBehaviour
{
    [Header("Movement Bounds")]
    public Vector3 center = Vector3.zero;
    public Vector3 boxSize = new Vector3(20f, 10f, 20f);

    [Header("Water Constraints")]
    public float waterSurfaceY = 5f;   // must stay BELOW this
    public float groundY = -5f;        // must stay ABOVE this

    [Header("Movement")]
    public float maxSpeed = 3f;
    public float maxForce = 5f;
    public float arriveDistance = 1.5f;

    [Header("Targeting")]
    public float targetSwitchTime = 3f;

    private Vector3 velocity;
    private Vector3 acceleration;
    private Vector3 target;
    private float timer;

    void Start()
    {
        velocity = Random.insideUnitSphere;
        PickNewTarget();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= targetSwitchTime || Vector3.Distance(transform.position, target) < arriveDistance)
        {
            PickNewTarget();
            timer = 0f;
        }

        ApplySteering(Seek(target));
        ApplyWaterConstraints();

        UpdateMotion();
        FaceVelocity();
    }

    void PickNewTarget()
    {
        float minX = center.x - boxSize.x * 0.5f;
        float maxX = center.x + boxSize.x * 0.5f;

        float minZ = center.z - boxSize.z * 0.5f;
        float maxZ = center.z + boxSize.z * 0.5f;

        float minY = groundY;
        float maxY = waterSurfaceY;

        target = new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY),
            Random.Range(minZ, maxZ)
        );
    }

    Vector3 Seek(Vector3 tgt)
    {
        Vector3 desired = tgt - transform.position;
        float distance = desired.magnitude;

        desired.Normalize();

        // slow down when approaching target
        if (distance < arriveDistance)
        {
            desired *= maxSpeed * (distance / arriveDistance);
        }
        else
        {
            desired *= maxSpeed;
        }

        Vector3 steer = desired - velocity;
        return Vector3.ClampMagnitude(steer, maxForce);
    }

    void ApplySteering(Vector3 force)
    {
        acceleration += force;
    }

    void ApplyWaterConstraints()
    {
        Vector3 pos = transform.position;

        // soft constraint: push back into water volume
        if (pos.y > waterSurfaceY)
        {
            acceleration += Vector3.down * (pos.y - waterSurfaceY) * 2f;
        }
        else if (pos.y < groundY)
        {
            acceleration += Vector3.up * (groundY - pos.y) * 2f;
        }

        // small wandering noise for organic swimming
        acceleration += new Vector3(
            Mathf.PerlinNoise(Time.time, 0f) - 0.5f,
            Mathf.PerlinNoise(0f, Time.time) - 0.5f,
            Mathf.PerlinNoise(Time.time * 0.5f, Time.time * 0.5f) - 0.5f
        ) * 0.5f;
    }

    void UpdateMotion()
    {
        velocity += acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        transform.position += velocity * Time.deltaTime;

        acceleration = Vector3.zero;
    }

    void FaceVelocity()
    {
        if (velocity.sqrMagnitude > 0.001f)
        {
            Quaternion rot = Quaternion.LookRotation(velocity.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 5f);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(center, boxSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(
            new Vector3(center.x - boxSize.x, waterSurfaceY, center.z),
            new Vector3(center.x + boxSize.x, waterSurfaceY, center.z)
        );

        Gizmos.color = Color.green;
        Gizmos.DrawLine(
            new Vector3(center.x - boxSize.x, groundY, center.z),
            new Vector3(center.x + boxSize.x, groundY, center.z)
        );

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(target, 0.2f);
    }
}