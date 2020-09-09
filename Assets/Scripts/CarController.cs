using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
/* Wheel struct to allow of extendability of how wheels are handled, Axel is currently used to denote which set of wheels does the steering though that might change will probably add something to track which side the wheel is on.
 */
public struct Wheel
{
    public GameObject model;
    public WheelCollider collider;
    public Axel axel;
    public Side side;
}

public enum Axel
{
    Front,
    Rear
}
public enum Side
{
    Right,
    Left
}
public class CarController : MonoBehaviour
{

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
 
    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;

    private bool isBreaking;
    [SerializeField]
    private Vector3 _centerOfMass;
    private Rigidbody _rb;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField]
    private List<Wheel> wheels; //Wheels stored as a list to allow for vehicles with more than 4 wheels.
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.centerOfMass = _centerOfMass;
        foreach (Wheel wheel in wheels)
        {
            wheel.collider.ConfigureVehicleSubsteps(5f,12,15);
        }
    }
    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBreaking = Input.GetKey(KeyCode.Space);
    }
    private void Update()
    {
        GetInput();
        UpdateWheels();
    }
    private void FixedUpdate() 
    {       
        HandleMotor();
       // HandleSteering();      
    }
    //Spark if you are looking at this, its all a nightmare. Wheel colliders are a nightmare and this code is a hackjob.
    private void HandleMotor()
    {
        foreach (Wheel wheel in wheels)
        {
            wheel.collider.motorTorque = 0;
            wheel.collider.motorTorque = verticalInput * motorForce;
            if (horizontalInput != 0)
            {
                if (verticalInput >= 0)
                {

                    if (wheel.side == Side.Left)
                    {
                        wheel.collider.motorTorque += horizontalInput * (motorForce * ((verticalInput > 0) ? 3: 5) + (verticalInput * motorForce)/8);
                    }
                    else if (wheel.side == Side.Right)
                    {
                        wheel.collider.motorTorque -= horizontalInput * (motorForce* ((verticalInput > 0) ? 3 : 5) + (verticalInput * motorForce) / 8);
                    }
                }
                else if (verticalInput < 0)
                {
                    if (wheel.side == Side.Left)
                    {
                        wheel.collider.motorTorque -= horizontalInput * (motorForce*3 - (verticalInput * motorForce) /8);
                    }
                    else if (wheel.side == Side.Right)
                    {
                        wheel.collider.motorTorque += horizontalInput * (motorForce*3 - (verticalInput * motorForce) / 8);
                    }
                }
            }
        }

        ApplyBreaking(isBreaking ? breakForce : 0f);
        
    }
    private void ApplyBreaking(float _breakforce)
    {
        foreach (Wheel wheel in wheels)
        {
            wheel.collider.brakeTorque = _breakforce;          
        }
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        foreach (Wheel wheel in wheels)
        {
            if (wheel.axel==Axel.Front)
                wheel.collider.steerAngle = currentSteerAngle;
        }
    }

    private void UpdateWheels()
    {
        foreach (Wheel wheel in wheels)
        {
            Vector3 pos;
            Quaternion rot;
            wheel.collider.GetWorldPose(out pos,out rot);
            wheel.model.transform.rotation = rot;
            wheel.model.transform.position = pos;
        }

    }

}
