//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;
//namespace BLACK.Core
//{
//    /*[Serializable]
//    public class SpringsAndWheels //TODO: Break out more things into the sub classes, its annoying while writing but I feel makes the code more readable as well as make the inspector more useful
//    {
//        public Transform springBR, springBL, springFL, springFR;//suspension spring locations
//        public Transform wheelBR, wheelBL, wheelFL, wheelFR;//Wheel positions
//    }*/
//    public class Suspension : MonoBehaviour
//    {
//        [SerializeField]
//        [Tooltip("How much force the spring exerts while fully compressed")]
//        public float suspensionForce = 50000f; //how much force the springs impart when fully compressed
//        [SerializeField]
//        [Tooltip("How long the suspension springs are")]
//        private float springDistance = 1.2f;//Length of springs
//        [SerializeField]
//        [Tooltip("How much the force pushing down on the suspention when moving forward or back.")]
//        private float wheelOffset = 1.8f;
//        [SerializeField]
//        [Tooltip("How much local vertical velocity is kept per physics tick, used in reducing bounce on the springs. only works while grounded")]
//        private float stiffness = 3f;//decay of vertical momentum while grounded.
//        [SerializeField]
//        [Tooltip("How much local side velocity is kept per physics tick, higher values = more drift. . only works while grounded")]
//        private float sideDrag = 4f;//Decay of sideways momentum while grounded
//        [SerializeField]
//        [Tooltip("How much local frontal velocity is kept per physics tick while accelerating, effects max speed. only works while grounded")]
//        private float frontDrag = 1f;//decay of vertical momentum while grounded.
       
//        private RaycastHit _springBR, _springBL, _springFR, _springFL;
//        public SpringsAndWheels springsandwheels;
//        private Rigidbody _rb;//penis joke


//        void Start()
//        {
//            _rb = GetComponent<Rigidbody>();
//            _rb.centerOfMass = new Vector3(0,1f,0); //TODO: Make this relate to the centerofmass object. Currently is wonky when I try which is weird.
//        }

//        // Update is called once per frame
//        void FixedUpdate()
//        {
//            _Suspension();
//            if (Physics.Raycast(Front.position,-Front.transform.up,out ground,groundDistance) && Physics.Raycast(Back.position,-Back.transform.up,out ground2,groundDistance))
//            {

//                _VelocityDecay(1f - (sideDrag - (fastturn ? sideDragReduction : 0)) * Time.deltaTime,1f - stiffness * Time.deltaTime,1f - (inputY != 0 ? frontDrag * Time.deltaTime : frontStopDrag * Time.deltaTime));
//            }
//        }
//        private void _VelocityDecay(float decX,float decY,float decZ)
//        {
//            Vector3 vel = transform.InverseTransformDirection(_rb.velocity); //Convert the rigidbody's world velocity into a local space
//            vel = new Vector3(vel.x * decX,vel.y * decY,vel.z * decZ); //Decay the local sideways velocity
//            _rb.velocity = transform.TransformDirection(vel); //Convert and update local velocity to the world
//        }

//        private void _Suspension()
//        {
//            //Applies the suspention force to all of the springs!
//            _SpringForce(springsandwheels.springBL,springsandwheels.wheelBL,out _springBL);
//            _SpringForce(springsandwheels.springBR,springsandwheels.wheelBR,out _springBR);
//            _SpringForce(springsandwheels.springFL,springsandwheels.wheelFL,out _springFL);
//            _SpringForce(springsandwheels.springFR,springsandwheels.wheelFR,out _springFR);
//        }

//        private void _SpringForce(Transform spring,Transform wheel,out RaycastHit springcast)
//        {

//            //Check if the spring is contacting the ground
//            if (Physics.Raycast(spring.position,-spring.up,out springcast,springDistance))
//            {
//                Debug.DrawRay(spring.position,-spring.transform.up,Color.green);
//                /*
//                 * Added force at the position of the spring based on how compressed the spring is
//                 * (suspensionForce * (1 - bl.distance / springDistance)) this line is probably jank math
//                */
//                _rb.AddForceAtPosition(spring.up * (suspensionForce * (springDistance - springcast.distance / springDistance)),spring.position);
//                wheel.localPosition = new Vector3(wheel.localPosition.x,-(springcast.distance - wheelOffset),wheel.localPosition.z); //Makes the wheel match the spring position
//            }
//            else
//            {
//                Debug.DrawRay(spring.position,-spring.transform.up,Color.red);
//                wheel.localPosition = new Vector3(wheel.localPosition.x,-(springDistance - wheelOffset),wheel.localPosition.z);// if the spring doesn't connect assume the wheel is a full... uh spring
//            }
//        }
//    }
//}
