using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarFlipping : MonoBehaviour
{

    [SerializeField]
    Rigidbody _rb;
    public GameObject playerCar;
    private float timeSinceLastFlipped;
    private float timeTillUnflipped = 2.5f;
    public bool isFlipped;
    public float flippingForce = 1000;
    public float flippingTime = 2.5f;

    private void Start() 
    {
        _rb = GetComponent<Rigidbody>();
        timeSinceLastFlipped = Mathf.Infinity;
        isFlipped = false;
    }

    private void Update() 
    {
        //if car is at certain angle, set to isFlipped
        if (playerCar.transform.rotation.z >= 90 || playerCar.transform.rotation.z <= 270)
        {
            isFlipped = true;
        }
        if (isFlipped == true)
        {
            timeSinceLastFlipped = Time.time;
        if (timeSinceLastFlipped >= timeTillUnflipped && isFlipped == true)
        {
            isFlipped = false;
            _rb.AddRelativeTorque(Vector3.forward * flippingForce);
            return;
        }

    }
    
    }

}
