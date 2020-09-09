using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControllerFINAL : MonoBehaviour
{
    /*If this shit doesnt end up working I will commit mine <3 -shawn
    */
    [SerializeField]
    Transform springBR, springBL, springFL, springFR;
    [SerializeField]
    private float suspensionForce = 100f;
    [SerializeField]
    private float springDistance = 1f;
    [SerializeField]
    private float accel = 300;
    [SerializeField]
    private float turn = 300;

    private Rigidbody _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Suspension();
        Thrust();//( ͡° ͜ʖ ͡°)
    }

    private void Thrust()
    {
        Vector3 vel = transform.InverseTransformDirection(_rb.velocity);
        vel = new Vector3(vel.x * .8f,vel.y,vel.z);
        _rb.velocity = transform.TransformDirection(vel);
        float inputY = Input.GetAxis("Vertical");
        _rb.AddRelativeForce(Vector3.forward * accel * inputY);
        float inputX = Input.GetAxis("Horizontal");
        _rb.AddRelativeTorque(Vector3.up* turn * inputX);
    }

    private void Suspension()
    {
        RaycastHit br, bl, fl, fr;
        if (Physics.Raycast(springBL.position,-springBL.up,out bl,springDistance))
        {
            _rb.AddForceAtPosition(springBL.up * (suspensionForce * (1-bl.distance/ springDistance)),springBL.position);
        }
        if (Physics.Raycast(springBR.position,-springBR.up,out br,springDistance))
        {
            _rb.AddForceAtPosition(springBR.up * (suspensionForce * (1 - br.distance / springDistance)),springBR.position);
        }
        if (Physics.Raycast(springFL.position,-springBL.up,out fl,springDistance))
        {
            _rb.AddForceAtPosition(springFL.up * (suspensionForce * (1 - fl.distance / springDistance)),springFL.position);
        }
        if (Physics.Raycast(springFR.position,-springFR.up,out fr,springDistance))
        {
            _rb.AddForceAtPosition(springFR.up * (suspensionForce * (1 - fr.distance / springDistance)),springFR.position);
        }
    }
}
