using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarFlipping : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    Rigidbody _rb;
    public GameObject playerCar;
    private float timeSinceLastFlipped;
    private float timeTillUnflipped = 2.5f;
    private bool isFlipped;
    public float flippingForce = 10000;
    public float flippingTime = 2.5f;

    private void Start()
    {

        _rb = GetComponent<Rigidbody>();
        timeSinceLastFlipped = 0;
        isFlipped = false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        //if car is at certain angle, set to isFlipped
        float zAngle = playerCar.transform.rotation.eulerAngles.z;
       // print(zAngle);
        if (zAngle >= 80 && zAngle <= 280)
        {
            isFlipped = true;
        }
        else
        {
            timeSinceLastFlipped = 0;
            isFlipped = false;
        }
        if (isFlipped == true)
        {
            timeSinceLastFlipped += Time.deltaTime;
            if (timeSinceLastFlipped >= timeTillUnflipped && isFlipped == true)
            {
                
                //isFlipped = false;
                _rb.AddRelativeTorque(Vector3.forward * (zAngle>180?flippingForce:-flippingForce));
                return;
            }

        }

    }

}