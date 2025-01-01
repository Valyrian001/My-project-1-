using Unity.Netcode;
using UnityEngine;

public class CarController : NetworkBehaviour
{
    [SerializeField] private WheelCollider frontLeft;
    [SerializeField] private WheelCollider frontRight;
    [SerializeField] private WheelCollider rearLeft;
    [SerializeField] private WheelCollider rearRight;

    [SerializeField] private Transform frontLeftTransform;
    [SerializeField] private Transform frontRightTransform;
    [SerializeField] private Transform rearLeftTransform;
    [SerializeField] private Transform rearRightTransform;

    public float acceleration = 800f;
    public float brakeForce = 600f;
    public float maxTurnAngle = 10f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Debug.Log($"Car spawned for owner {OwnerClientId}");
            ResetCarPhysics();
        }
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return; // Only the owner controls the car

        HandleAcceleration();
        HandleSteering();
        HandleBraking();
    }

    private void HandleAcceleration()
    {
        float accelerationInput = Input.GetAxis("Vertical");
        frontLeft.motorTorque = acceleration * accelerationInput;
        frontRight.motorTorque = acceleration * accelerationInput;
        rearLeft.motorTorque = acceleration * accelerationInput;
        rearRight.motorTorque = acceleration * accelerationInput;
    }

    private void HandleSteering()
    {
        float steeringInput = Input.GetAxis("Horizontal");
        float turnAngle = maxTurnAngle * steeringInput;
        frontLeft.steerAngle = turnAngle;
        frontRight.steerAngle = turnAngle;
    }

    private void HandleBraking()
    {
        bool isBraking = Input.GetKey(KeyCode.Space);
        float brake = isBraking ? brakeForce : 0f;

        frontLeft.brakeTorque = brake;
        frontRight.brakeTorque = brake;
        rearLeft.brakeTorque = brake;
        rearRight.brakeTorque = brake;
    }

    private void ResetCarPhysics()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero; // Fixed typo from 'linearVelocity'
            rb.angularVelocity = Vector3.zero;
        }
    }
}
