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
        #region Variables 
        [SerializeField]
        Transform springBR, springBL, springFL, springFR; //Suspension spring locations
        [SerializeField]
        Transform wheelBR, wheelBL, wheelFL, wheelFR;//Wheel positions
        [SerializeField]
        private float suspensionForce = 100f; //how much force the springs impart when fully compressed
        [SerializeField]
        private float springDistance = 1f;//Length of springs
        [SerializeField]
        private float groundDistance = 3f;//How from the ground the car still does car things
        [SerializeField]
        private float accel = 300;//Force Accel of the car
        [SerializeField]
        private float turn = 300;//Turn Torque
        [SerializeField]
        private float stiffness = 0.8f;//Turn Torque
        private Rigidbody _rb;//heh he said Rigid
        #endregion

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
            float inputY = Input.GetAxis("Vertical");
            float inputX = Input.GetAxis("Horizontal");

            //Check to see if the car is more or less touching the ground
            RaycastHit ground;
            if (Physics.Raycast(_rb.position,-_rb.transform.up,out ground,groundDistance))
            {
                Vector3 vel = transform.InverseTransformDirection(_rb.velocity); //Convert the rigidbodie's world velocity into a local space
                vel = new Vector3(vel.x * .8f,vel.y,vel.z); //Decay the local sideways velocity
                _rb.velocity = transform.TransformDirection(vel); //Convert and update local velocity to the world
                // _rb.AddRelativeForce(ground.transform.forward * accel * inputY);
                _rb.AddRelativeForce(Vector3.forward * accel * inputY);//Move the car
                if (inputY > 0)
                {

                    _rb.AddForceAtPosition((-springFL.up * suspensionForce / 4) * inputY,springFL.position);
                    _rb.AddForceAtPosition((-springFR.up * suspensionForce / 4) * inputY,springFR.position);
                }
                if (inputY < 0)
                {
                    _rb.AddForceAtPosition((-springBL.up * suspensionForce / 4) * -inputY,springBL.position);
                    _rb.AddForceAtPosition((-springBR.up * suspensionForce / 4) * -inputY,springBR.position);
                }
                if (inputX > 0)
                {
                    _rb.AddForceAtPosition((-springBR.up * suspensionForce / 4) * inputX,springBR.position);
                 
                    _rb.AddForceAtPosition((-springFR.up * suspensionForce / 4) * inputX,springFR.position);
                }
                if (inputX < 0)
                {
                    _rb.AddForceAtPosition((-springFL.up * suspensionForce / 4) * -inputX,springFL.position);
                    _rb.AddForceAtPosition((-springBL.up * suspensionForce / 4) * -inputX,springBL.position);
                    
                }
                //_rb.AddRelativeTorque(Vector3.right * 10000f * inputY);

            }
            _rb.AddRelativeTorque(Vector3.up * turn * inputX);//Spin the Car
        }

        private void Suspension()
        {

            RaycastHit br, bl, fl, fr;
            //Check if the spring is contacting thhe ground
            if (Physics.Raycast(springBL.position,-springBL.up,out bl,springDistance))
            {
                /*
                 * Added force at the position of the spring based on how compressed the spring is
                 * (suspensionForce * (1 - bl.distance / springDistance)) this line is probably jank math
                */
                _rb.AddForceAtPosition(springBL.up * ((suspensionForce * ((float) Math.Exp(springDistance  -bl.distance / springDistance) / 2))),springBL.position);
                wheelBL.localPosition = new Vector3(wheelBL.localPosition.x,-(bl.distance - .25f),wheelBL.localPosition.z); //Makes the wheel match the spring position
            }
            else
            {
                wheelBL.localPosition = new Vector3(wheelBL.localPosition.x,-(springDistance - .25f),wheelBL.localPosition.z);
            }
            if (Physics.Raycast(springBR.position, -springBR.up, out br, springDistance))
            {
                _rb.AddForceAtPosition(springBR.up * ((suspensionForce * ((float) Math.Exp(springDistance  -br.distance / springDistance) / 2))), springBR.position);
                wheelBR.localPosition = new Vector3(wheelBR.localPosition.x, -(br.distance - .25f), wheelBR.localPosition.z);
            }
            else
            {
                wheelBR.localPosition = new Vector3(wheelBR.localPosition.x,-(springDistance - .25f),wheelBR.localPosition.z);
            }
            if (Physics.Raycast(springFL.position, -springFL.up, out fl, springDistance))
            {
                _rb.AddForceAtPosition(springFL.up * ((suspensionForce * ((float) Math.Exp(springDistance -fl.distance / springDistance) / 2))), springFL.position);
                wheelFL.localPosition = new Vector3(wheelFL.localPosition.x, -(fl.distance-.25f ), wheelFL.localPosition.z);
            }
            else
            {
                wheelFL.localPosition = new Vector3(wheelFL.localPosition.x,-(springDistance - .25f),wheelFL.localPosition.z);
            }
            if (Physics.Raycast(springFR.position, -springFR.up, out fr, springDistance))
            {
                _rb.AddForceAtPosition(springFR.up * ((suspensionForce * ((float)Math.Exp(springDistance - fr.distance / springDistance)/2))), springFR.position);
                wheelFR.localPosition = new Vector3(wheelFR.localPosition.x, -(fr.distance - .25f), wheelFR.localPosition.z);
            }
            else
            {
                wheelFR.localPosition = new Vector3(wheelFR.localPosition.x,-(springDistance - .25f),wheelFR.localPosition.z);
            }
        }
    }

}