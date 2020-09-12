using System.Collections;
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
        private BLACK.Combat.Dumbfire missileBase;
        private bool following = true;
        void Start()
        {
            missileBase = GetComponent<Dumbfire>();
            BLACK.Core.CarController car = missileBase.GetParent().GetComponent<BLACK.Core.CarController>();
            following = car.isGrounded();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            RaycastHit ray, ray2;
            Debug.DrawRay(transform.position,Vector3.down);
            Debug.DrawRay(transform.position - transform.forward,Vector3.down);
            if (Physics.Raycast(transform.position,Vector3.down,out ray,heightThresh) && Physics.Raycast(transform.position + transform.forward,Vector3.down,out ray2,heightThresh) && following)
            {
                //Correct the rotation of the missile to level out with the ground
                transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.LookRotation(ray2.point - ray.point,Vector3.up),anglecorrect); 
              
                if (ray.distance > height) //if missile is too high
                {
                    float tempCorrect = Mathf.Clamp(correct,0,ray.distance - height); //Clamp Correction Value to the max needed to correct
                    transform.position -= new Vector3(0,tempCorrect,0); //apply adjustment
                }
                else if (ray.distance < height) //if missile is too low
                {
                    float tempCorrect = Mathf.Clamp(correct,0,height - ray.distance);
                    transform.position += new Vector3(0,tempCorrect,0);
                }
            }
            else
            {
                following = false; //if the floor has been lost, never correct to it again
            }
        }
    }
}
