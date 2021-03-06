﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BLACK.Combat
{
    [RequireComponent(typeof(BLACK.Combat.Dumbfire))]
    public class groundfollow : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField]
        private float height = 2;
        [SerializeField]
        [Tooltip("Amount the height of the missile is adjusted per tick")]
        private float correct = .5f;
        [SerializeField]
        [Tooltip("Maximum angle correction per tick")]
        private float anglecorrect = .5f;
        [SerializeField]
        [Tooltip("How far from the ground the missile need to be before it stops trying to follow it")]
        private float heightThresh = 10f;
        [SerializeField]
        private float delay = 0f;
        [SerializeField]
        private float duration = -1;

        private BLACK.Combat.Dumbfire _missileBase;
        private bool following = true;
        void Start()
        {
            _missileBase = GetComponent<Dumbfire>();
            BLACK.Core.CarController car = _missileBase.GetParent().GetComponent<BLACK.Core.CarController>();
            following = car.isGrounded();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            float heightFromTarget = 0;
            if (_missileBase.target != null && _missileBase.Targeting)
            {
                heightFromTarget = transform.position.y - _missileBase.target.transform.position.y;
            }
            if (_missileBase.aliveTime > delay && _missileBase.aliveTime < (duration < 0 ? Mathf.Infinity: duration+delay)){

                RaycastHit ray, ray2;
                Debug.DrawRay(transform.position,Vector3.down);
                Debug.DrawRay(transform.position - transform.forward,Vector3.down);
                if (Physics.Raycast(transform.position,Vector3.down,out ray,heightThresh) && Physics.Raycast(transform.position + transform.forward,Vector3.down,out ray2,heightThresh) && following)
                {
                    //Correct the rotation of the missile to level out with the ground
                    if(_missileBase.target!=null || !_missileBase.Targeting)
                    transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.LookRotation(ray2.point - ray.point,Vector3.up),anglecorrect * Time.deltaTime);

                    if (ray.distance > height&& heightFromTarget <= 0) //if missile is too high
                    {
                        float tempCorrect = Mathf.Clamp(correct * Time.deltaTime,0,ray.distance - height); //Clamp Correction Value to the max needed to correct
                        transform.position -= new Vector3(0,tempCorrect,0); //apply adjustment
                    }
                    else if (ray.distance < height) //if missile is too low
                    {
                        float tempCorrect = Mathf.Clamp(correct * Time.deltaTime,0,height - ray.distance);
                        transform.position += new Vector3(0,tempCorrect ,0);
                    }
                }
                else
                {
                    following = false; //if the floor has been lost, never correct to it again
                }
            }
        }
    }
}
