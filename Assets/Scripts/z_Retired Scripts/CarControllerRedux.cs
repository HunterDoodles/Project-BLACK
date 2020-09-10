using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControllerRedux : MonoBehaviour
{
    //You are going to get some quick and dirty comments for now I'll make it readable later
    //Also all this fucking about and I have <40 lines of code, I hate physics
    [SerializeField]
    private float speed = 100f;
    [SerializeField]
    private float maxspeed = 200f;
    [SerializeField]
    private float rotatespeed = 10f;
    [SerializeField]
    private float decay = 0.90f; //This value is a artificial drag value that does not interfere with the gravity
    [SerializeField]
    private Transform CenterOfMass;
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
      //  _rb.centerOfMass = CenterOfMass.position;
    }

    
    void FixedUpdate() //Fixed update is called everytime physics is so this should be used for anything involving the physics
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

       // _rb.velocity=new Vector3(_rb.velocity.x* decay,_rb.velocity.y * decay,_rb.velocity.z * decay);//Drag in the x,z axis.
        
        Vector3 movement = transform.forward *(vertical * speed);
       
        RaycastHit hit; //Stored but not yet used
        if (Physics.Raycast(transform.position,transform.TransformDirection(Vector3.down),out hit,1.2f))
        {
            _rb.AddForce(movement,ForceMode.VelocityChange); //FM Velocity Change makes the change ignore mass
            _rb.AddRelativeForce(Vector3.down*5,ForceMode.VelocityChange);
            Vector3 TempVel = Vector3.ClampMagnitude(new Vector3(_rb.velocity.x,0,_rb.velocity.z),maxspeed); //Clamping the max value of speed in the x,z axis this is a hackjob and is terrible though unrelated to the issue with the ramps
            _rb.velocity = new Vector3(TempVel.x,_rb.velocity.y,TempVel.z);//apply the velocity
            //_rb.velocity = movement;
        }
        Quaternion rotation = transform.rotation * Quaternion.Euler(0,rotatespeed * horizontal * Time.deltaTime,0);//Quaternions scare me
        //_rb.MovePosition(transform.position + movement);
        _rb.MoveRotation(rotation);//This is a sin you go to hell for this, directly rotating stuff is apperently a bad idea, but it works so its low priorty to fix, I can't wait until I forget about this hackjob and its a problem later.
        
    }
}
