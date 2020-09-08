using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentBreakForce;
    private bool isBreaking;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider Wheel_FR_Collider;
    [SerializeField] private WheelCollider Wheel_FL_Collider;
    [SerializeField] private WheelCollider Wheel_BR_Collider;
    [SerializeField] private WheelCollider Wheel_BL_Collider;

    [SerializeField] private Transform Wheel_FR_Transform;
    [SerializeField] private Transform Wheel_FL_Transform;
    [SerializeField] private Transform Wheel_BR_Transform;
    [SerializeField] private Transform Wheel_BL_Transform;

    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBreaking = Input.GetKey(KeyCode.Space);
    }
    
    private void FixedUpdate() 
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();

    }
    
    private void HandleMotor()
    {
        Wheel_FR_Collider.motorTorque = verticalInput * motorForce;
        Wheel_FL_Collider.motorTorque = verticalInput * motorForce;
        breakForce = isBreaking ? breakForce : 0f;
        if(isBreaking)
        {
            ApplyBreaking();
        }
    }
    private void ApplyBreaking()
    {
        Wheel_FR_Collider.brakeTorque = currentBreakForce;
        Wheel_FL_Collider.brakeTorque = currentBreakForce;
        Wheel_BR_Collider.brakeTorque = currentBreakForce;
        Wheel_BL_Collider.brakeTorque = currentBreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        Wheel_FR_Collider.steerAngle = currentSteerAngle;
        Wheel_FL_Collider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(Wheel_FR_Collider, Wheel_FR_Transform);
        UpdateSingleWheel(Wheel_FL_Collider, Wheel_FL_Transform);
        UpdateSingleWheel(Wheel_BR_Collider, Wheel_BR_Transform);
        UpdateSingleWheel(Wheel_BL_Collider, Wheel_BL_Transform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

}
