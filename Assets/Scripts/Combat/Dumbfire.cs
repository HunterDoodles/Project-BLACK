using System;
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
        public Target target;
        private ParticleSystem emit;
        public float aliveTime = 0;
        void Start()
        {
            emit = GetComponentInChildren<ParticleSystem>();
            // GetComponent<Rigidbody>().detectCollisions = false;
        }


        void Update()
        {
            aliveTime += Time.deltaTime;
            transform.position += transform.forward * speed*Time.deltaTime;
        }
        //https://www.youtube.com/watch?v=lLl0DVzRksk
        public void SetParent(GameObject parentalfigure)
        {
            _parent = parentalfigure;
        }
        public GameObject GetParent()
        {
            return _parent;
        }
        private void OnTriggerEnter(Collider other)
        {
            //Make sure you are not hitting daddy, he doesnt like that. Also make sure you are not colliding with fellow projectiles
            if (other.gameObject != _parent && other.gameObject.tag != "Projectiles")
            {
                Rigidbody CollidedRB = other.gameObject.GetComponent<Rigidbody>();
                if (CollidedRB != null)
                { //Ensure we are actually dealing with something rigid
                    other.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(transform.forward * impact,other.ClosestPointOnBounds(transform.position)); //if so apply force at point of contact (more or less)
                }
                if (emit != null)
                {
                    DetachParticle();
                }
                Destroy(gameObject); //Dead... not big surprise.
            }
        }

        private void DetachParticle()
        {
            emit.transform.parent = null;
            emit.Stop();
            Destroy(emit.gameObject,5);
        }
    }
}
