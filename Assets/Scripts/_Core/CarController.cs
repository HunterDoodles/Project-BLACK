using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BLACK.Core
{
    [Serializable]
    public class SpringsAndWheels //TODO: Break out more things into the sub classes, its annoying while writing but I feel makes the code more readable as well as make the inspector more useful
    {
        public Transform springBR, springBL, springFL, springFR;//suspension spring locations
        public Transform wheelBR, wheelBL, wheelFL, wheelFR;//Wheel positions
    }
    public class CarController : MonoBehaviour
    {
        /*If this shit doesnt end up working I will commit mine <3 -shawn
        */
        #region Variables 
        public SpringsAndWheels springsandwheels;

        [SerializeField]
        [Tooltip("How much force the spring exerts while fully compressed")]
        private float suspensionForce = 30000f; //how much force the springs impart when fully compressed
        [SerializeField]
        [Tooltip("How long the suspension springs are")]
        private float springDistance = 1.2f;//Length of springs
        [SerializeField]
        [Tooltip("How far from the ground you can be and still be considered grounded")]
        private float groundDistance = 1.5f;//How from the ground the car still does car things
        [SerializeField]
        private float accelForce = 100000;//Force Accel of the car
        [SerializeField]
        private float turningTorque = 250000;//Turn Torque
        [SerializeField]
        [Tooltip("How much local vertical velocity is kept per physics tick, used in reducing bounce on the springs. only works while grounded")]
        private float stiffness = 5f;//decay of vertical momentum while grounded.
        [SerializeField]
        [Tooltip("How much local side velocity is kept per physics tick, higher values = more drift. . only works while grounded")]
        private float sideDrag = 5f;//Decay of sideways momentum while grounded
        [SerializeField]
        private float sideDragReduction= 2.5f;//Decay of sideways momentum while grounded
        [SerializeField]
        [Tooltip("How much local frontal velocity is kept per physics tick while accelerating, effects max speed. only works while grounded")]
        private float frontDrag = 1f;//decay of vertical momentum while grounded.
        [SerializeField]
        [Tooltip("How much local frontal velocity is kept per physics tick while you are not accelerating. only works while grounded")]
        private float frontStopDrag = 2.5f;
        [SerializeField]
        [Tooltip("Caps the fall speed, test throughly if you mess with this, high speed break collision.")]
        private float maxFallSpeed = 35f;
        [SerializeField]
        [Tooltip("How much the force pushing down on the suspention when moving forward or back.")]
        private float wheelOffset = .4f; 
        [SerializeField]
        [Tooltip("How much the force pushing down on the suspention when moving forward or back.")]
        private float thrustSpringFactor = 6f;
        [SerializeField]
        [Tooltip("How much the force pushing down on the suspention when turning.")]
        private float turnSpringFactor = 8f;
        [SerializeField]
        private Transform centerOfMass;
        [SerializeField]
        private Transform Front, Back;

        private RaycastHit _springBR, _springBL, _springFR, _springFL;
        private Vector3 _groundVector;
        private Rigidbody _rb;//penis joke
        private bool _grounded = false;
        #endregion

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.centerOfMass = new Vector3(0,1f,0); //TODO: Make this relate to the centerofmass object. Currently is wonky when I try which is weird.
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            _Suspension();
            _Thrust();//( ͡° ͜ʖ ͡°)
        }
        public Vector3 getGroundForward()
        {
            return _groundVector;
        }
        public bool isGrounded()
        {
            return _grounded;
        }
        private void _Thrust()
        {
            float inputY = Input.GetAxis("Vertical");//TODO: Find a better way to wrap inputs, I fucking hate unity's default system
            float inputX = Input.GetAxis("Horizontal");
            bool boosting = Input.GetKey("k");
            bool fastturn = Input.GetKey("l");
            //Check to see if the car is more or less touching the ground
            RaycastHit ground,ground2;
            Debug.DrawRay(Front.position,-Front.transform.up);
            Debug.DrawRay(Back.position,-Back.transform.up);
           
            if (Physics.Raycast(Front.position,-Front.transform.up,out ground,groundDistance)&& Physics.Raycast(Back.position,-Back.transform.up,out ground2,groundDistance))
            {
                _grounded = true;
              
                //here we are basically looking at the line made by the ground at the front of the car and the back of it to get the angle of the ground.
                if (Physics.Raycast(_rb.position -_rb.transform.forward*.02f,-_rb.transform.up,out ground2,groundDistance))
                {
                    _groundVector = ground.point - ground2.point;
                    _groundVector.Normalize();
                }

                //_VelocityDecay((1f-(sideDrag-(fastturn?sideDragReduction:0)))*Time.deltaTime,1f-stiffness * Time.deltaTime,1f-(inputY != 0 ? frontDrag * Time.deltaTime : frontStopDrag * Time.deltaTime));
                _VelocityDecay(1f - (sideDrag-(fastturn?sideDragReduction:0) ) * Time.deltaTime,1f - stiffness * Time.deltaTime,1f - (inputY != 0 ? frontDrag * Time.deltaTime : frontStopDrag * Time.deltaTime));
                _rb.AddForce(_groundVector * accelForce * inputY * (boosting ? 1.5f : 1));//Apply forward thrust to the car, increase that by 50% if you are boosting that value subject to change. force is applied parallel to the ground.
                if (Input.GetKeyDown("f"))
                {
                    _rb.AddForce(transform.up * 2000000);
                }
                else
                {
                }
                _AnimateMovement(inputX,inputY,boosting);//Do the silly car leaning thing

            }
            else
            {
                _grounded = false;
                _FallClamp();
            }
            _rb.AddRelativeTorque(Vector3.up * turningTorque *(fastturn?1.5f:1) * inputX);//Spin the Car
        }

        private void _FallClamp()
        {
            Vector3 vel = transform.InverseTransformDirection(_rb.velocity); //Convert the rigidbody's world velocity into a local space            
            vel = new Vector3(vel.x,Mathf.Clamp(vel.y,-maxFallSpeed,Mathf.Infinity),vel.z); //Clamp the -y speed to cap the fall speed
            _rb.velocity = transform.TransformDirection(vel);//Reapply velocity in the world space 
        }

        private void _VelocityDecay(float decX,float decY,float decZ)
        {
            Vector3 vel = transform.InverseTransformDirection(_rb.velocity); //Convert the rigidbody's world velocity into a local space
            vel = new Vector3(vel.x * decX,vel.y * decY,vel.z * decZ); //Decay the local sideways velocity
            _rb.velocity = transform.TransformDirection(vel); //Convert and update local velocity to the world
        }

        private void _AnimateMovement(float inputx,float inputy,bool boosting)
        {
            /*
             * This code basically just pushes on the springs when you move or turn
            */
            if (inputy > 0)
            {
                _SpringPush(springsandwheels.springFL,springsandwheels.springFR,thrustSpringFactor,inputy * (boosting ? 2 : 1)); //Push front springs
            }
            if (inputy < 0)
            {
                _SpringPush(springsandwheels.springBL,springsandwheels.springBR,thrustSpringFactor,-inputy * (boosting ? 2 : 1));//Push back springs
            }
            if (inputx > 0)
            {
                _SpringPush(springsandwheels.springBR,springsandwheels.springFR,turnSpringFactor,inputx);//push right springs

            }
            if (inputx < 0)
            {
                _SpringPush(springsandwheels.springBL,springsandwheels.springFL,turnSpringFactor,-inputx);//push... you guessed it left springs
            }

        }

        private void _SpringPush(Transform spring1,Transform spring2,float springfactor,float input)
        {
            /*
             * Divided the suspension force by a preset number and pushes down on two springs
             * Should theoretically make the animation scale regardless of vehicles... I hope
            */
            _rb.AddForceAtPosition((-spring1.up * suspensionForce / springfactor) * input,spring1.position);
            _rb.AddForceAtPosition((-spring2.up * suspensionForce / springfactor) * input,spring2.position);
        }

        private void _Suspension()
        {
            //Applies the suspention force to all of the springs!
            _SpringForce(springsandwheels.springBL,springsandwheels.wheelBL,out _springBL);
            _SpringForce(springsandwheels.springBR,springsandwheels.wheelBR,out _springBR);
            _SpringForce(springsandwheels.springFL,springsandwheels.wheelFL,out _springFL);
            _SpringForce(springsandwheels.springFR,springsandwheels.wheelFR,out _springFR);
        }

        private void _SpringForce(Transform spring,Transform wheel,out RaycastHit springcast)
        {

            //Check if the spring is contacting the ground
            if (Physics.Raycast(spring.position,-spring.up,out springcast,springDistance))
            {
                Debug.DrawRay(spring.position,-spring.transform.up,Color.green);
                /*
                 * Added force at the position of the spring based on how compressed the spring is
                 * (suspensionForce * (1 - bl.distance / springDistance)) this line is probably jank math
                */
                _rb.AddForceAtPosition(spring.up * (suspensionForce * (springDistance - springcast.distance / springDistance)),spring.position);
                wheel.localPosition = new Vector3(wheel.localPosition.x,-(springcast.distance - wheelOffset),wheel.localPosition.z); //Makes the wheel match the spring position
            }
            else
            {
                Debug.DrawRay(spring.position,-spring.transform.up,Color.red);
                wheel.localPosition = new Vector3(wheel.localPosition.x,-(springDistance - wheelOffset),wheel.localPosition.z);// if the spring doesn't connect assume the wheel is a full... uh spring
            }
        }
    }

}