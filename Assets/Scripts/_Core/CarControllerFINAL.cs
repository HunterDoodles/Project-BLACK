using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BLACK.Core
{
    public class CarControllerFINAL : MonoBehaviour
    {
        /*If this shit doesnt end up working I will commit mine <3 -shawn
        */
        [SerializeField]
        Transform springBR, springBL, springFL, springFR;
        [SerializeField]
        Transform wheelBR, wheelBL, wheelFL, wheelFR;
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
            vel = new Vector3(vel.x * .8f, vel.y, vel.z);
            _rb.velocity = transform.TransformDirection(vel);
            float inputY = Input.GetAxis("Vertical");
            _rb.AddRelativeForce(Vector3.forward * accel * inputY);
            float inputX = Input.GetAxis("Horizontal");
            _rb.AddRelativeTorque(Vector3.up * turn * inputX);
        }

        private void Suspension()
        {
            RaycastHit br, bl, fl, fr;
            if (Physics.Raycast(springBL.position, -springBL.up, out bl, springDistance))
            {
                _rb.AddForceAtPosition(springBL.up * (suspensionForce * (1 - bl.distance / springDistance)), springBL.position);
                wheelBL.localPosition = new Vector3(wheelBL.localPosition.x, -(bl.distance / 2), wheelBL.localPosition.z);
            }
            if (Physics.Raycast(springBR.position, -springBR.up, out br, springDistance))
            {
                _rb.AddForceAtPosition(springBR.up * (suspensionForce * (1 - br.distance / springDistance)), springBR.position);
                wheelBR.localPosition = new Vector3(wheelBR.localPosition.x, -(br.distance / 2), wheelBR.localPosition.z);
            }
            if (Physics.Raycast(springFL.position, -springBL.up, out fl, springDistance))
            {
                _rb.AddForceAtPosition(springFL.up * (suspensionForce * (1 - fl.distance / springDistance)), springFL.position);
                wheelFL.localPosition = new Vector3(wheelFL.localPosition.x, -(fl.distance / 2), wheelFL.localPosition.z);
            }
            if (Physics.Raycast(springFR.position, -springFR.up, out fr, springDistance))
            {
                _rb.AddForceAtPosition(springFR.up * (suspensionForce * (1 - fr.distance / springDistance)), springFR.position);
                wheelFR.localPosition = new Vector3(wheelFR.localPosition.x, -(fr.distance / 2), wheelFR.localPosition.z);
            }
        }
    }

}