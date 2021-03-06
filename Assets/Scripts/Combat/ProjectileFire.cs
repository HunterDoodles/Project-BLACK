﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;
namespace BLACK.Combat
{
   
    [Serializable]
    public struct Missile
    {
        public string name;
        public Transform gunorigin;
        public GameObject projectile;
        public float spray;
        public float firedelay;
        public AudioClip Fire;
        public float soundPitch;
        public float soundVolume;
        public int ammo;
        public bool volleying;
        public int volleyCount;
        public float volleyDelay;
    }
    [RequireComponent(typeof(BLACK.Core.CarController))]
    public abstract class ProjectileFire : MonoBehaviour
    {
        [SerializeField]
        private Missile Bullet;
        /* [SerializeField]
         Transform gunorigin;
         [SerializeField]
         private GameObject projectile;
         [SerializeField]
         [Tooltip("Random bullet spread angle")]
         private float spray = 0f;
         [SerializeField]
         [Tooltip("In milliseconds")]
         private float firedelay = 500f;

         [SerializeField]
         private AudioClip Fire;
         [SerializeField]
         private float soundPitch = 1f;
         [SerializeField]
         private float soundVolume = 1f;*/
        [SerializeField]
        [Tooltip("This is a hack don't let me do this please")]//TODO stop committing crimes against code
        private string FireKey = "space";
        private AudioSource audioSource;
        private AcquireTargets AcTarg;
        private float _timesincefired;
        private BLACK.Core.CarController _control;
        public bool volleying = false;
        public bool holdDown = false;
        private int volleyCount = 0;
       // private float volleyDelay = 50f;
        void Start()
        {
            AcTarg = GetComponent<AcquireTargets>();
            audioSource = GetComponent<AudioSource>();
            _control = gameObject.GetComponent<BLACK.Core.CarController>();
            _timesincefired = Bullet.firedelay; //this is to ensure the player can fire immediately switching to the weapon
            Init();
        }

        protected abstract void Init();
        // Update is called once per frame
        void Update()
        {
            Update2();
            _timesincefired += Time.deltaTime * 1000;
            
            if (((holdDown?Input.GetKey(FireKey): Input.GetKeyDown(FireKey)) ||volleying) && _timesincefired >= (volleying?Bullet.volleyDelay:Bullet.firedelay)&&isLoaded())
            {
                _FireMissile();
            }

        }
        private void _FireMissile()
        {
            if (volleyCount > 0&& volleying)
            {
                volleyCount--;
                if (volleyCount == 0)
                {
                    volleying = false;
                }
            }
            else if (volleyCount == 0)
            {
                volleyCount = Bullet.volleyCount;
                volleying = Bullet.volleying;
                DecAmmo();
            }
           
            audioSource.pitch = Bullet.soundPitch;
            audioSource.PlayOneShot(Bullet.Fire,Bullet.soundVolume);
            _timesincefired = 0;
            UnityEngine.Random.InitState((int) (Time.frameCount * 100));
            //if grounded get the angle of the ground to determine the missile angle, if not just take it from the car
            Quaternion Rot = _control.isGrounded() ? Quaternion.LookRotation(_control.getGroundForward(),Vector3.up) * Quaternion.Euler(UnityEngine.Random.Range(-Bullet.spray,Bullet.spray),UnityEngine.Random.Range(-Bullet.spray,Bullet.spray),UnityEngine.Random.Range(-Bullet.spray,Bullet.spray)) : Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x + UnityEngine.Random.Range(-Bullet.spray,Bullet.spray),transform.rotation.eulerAngles.y + UnityEngine.Random.Range(-Bullet.spray,Bullet.spray),transform.rotation.eulerAngles.z + UnityEngine.Random.Range(-Bullet.spray,Bullet.spray)));
            GameObject child = Instantiate(Bullet.projectile,Bullet.gunorigin.position,Rot * Bullet.gunorigin.localRotation);
            NetworkServer.Spawn(child);
            Dumbfire dchild = child.GetComponent<Dumbfire>();
            dchild.target = AcTarg.GetCurrentTarget();
            dchild.SetParent(gameObject);
            
            
        }
        protected abstract void Update2();
        protected abstract bool isLoaded();
        protected abstract void DecAmmo();
        public void RackMissile(Missile missile)
        {
           /* gunorigin = missile.gunorigin;
            projectile = missile.projectile;
            spray = missile.spray;
            firedelay = missile.firedelay;
            Fire = missile.Fire;
            soundPitch = missile.soundPitch;
            soundVolume = missile.soundVolume;*/
            _timesincefired = 0;
            Bullet = missile;
        }
    }
   
}
