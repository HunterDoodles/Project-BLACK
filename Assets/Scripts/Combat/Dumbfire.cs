using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BLACK.Combat
{
    public class Dumbfire : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField]
        private float speed = 100f;
        [SerializeField]
        private float impact = 10000f; //force imparted on collision
        private GameObject _parent;
        void Start()
        {
            // GetComponent<Rigidbody>().detectCollisions = false;
        }


        void Update()
        {
            transform.position += transform.forward * speed*Time.deltaTime;
        }
        //https://www.youtube.com/watch?v=lLl0DVzRksk
        public void SetParent(GameObject parentalfigure)
        {
            _parent = parentalfigure; 
        }
        private void OnTriggerEnter(Collider other)
        {
            //Make sure you are not hitting daddy, he doesnt like that. Also make sure you are not colliding with fellow projectiles
            if (other.gameObject != _parent && other.gameObject.tag != "Projectiles")
            {
                Rigidbody CollidedRB = other.gameObject.GetComponent<Rigidbody>();
                if (CollidedRB != null) //Ensure we are actually dealing with something rigid
                    other.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(transform.forward * impact,other.ClosestPointOnBounds(transform.position)); //if so apply force at point of contact (more or less)
                Destroy(gameObject); //Dead... not big surprise.
            }
        }

    }
}
