using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BLACK.Combat
{
    [RequireComponent(typeof(BLACK.Core.CarController))]
    public class MachineGunFire : MonoBehaviour
    {
        
        [SerializeField]
        Transform gunorigin;
        [SerializeField]
        private GameObject projectile;
        [SerializeField]
        [Tooltip("Random bullet spread angle")]
        private float spray = 0f; 
        [SerializeField]
        [Tooltip("In milliseconds")]
        private float _firedelay = 500f;
        [SerializeField]
        [Tooltip("This is a hack don't let me do this please")]//TODO stop committing crimes against code
        private string FireKey = "space";
        [SerializeField]
        private AudioClip Fire;
        [SerializeField]
        private float soundPitch = 1f;
        [SerializeField]
        private float soundVolume = 1f;
        private AudioSource audioSource;

        private float _timesincefired;
        private BLACK.Core.CarController _control;
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            _control = gameObject.GetComponent<BLACK.Core.CarController>();
            _timesincefired = _firedelay; //this is to ensure the player can fire immediately switching to the weapon
        }

        // Update is called once per frame
        void Update()
        {
            _timesincefired += Time.deltaTime * 1000; 
            if (Input.GetKey(FireKey)&&_timesincefired>=_firedelay)
            {
                audioSource.pitch = soundPitch;
                audioSource.PlayOneShot(Fire,soundVolume);
                _timesincefired = 0; 
                Random.InitState((int) (Time.frameCount * 100)); 
                //if grounded get the angle of the ground to determine the missile angle, if not just take it from the car
                Quaternion Rot = _control.isGrounded() ? Quaternion.LookRotation(_control.getGroundForward(),Vector3.up)*Quaternion.Euler(Random.Range(-spray,spray),Random.Range(-spray,spray),Random.Range(-spray,spray)) : Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x + Random.Range(-spray,spray),transform.rotation.eulerAngles.y + Random.Range(-spray,spray),transform.rotation.eulerAngles.z + Random.Range(-spray,spray)));
                GameObject child = Instantiate(projectile,gunorigin.position,Rot);
                child.GetComponent<Dumbfire>().SetParent(gameObject);
            }
        }
    }
}
