using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BLACK.Core
{
    public class StableCamera : MonoBehaviour
    {

        // Obtain Car Object
        public GameObject playerCar;
        
        // Isolate Axis Values
        public float carX;
        public float carY;
        public float carZ;

        void Update()
        {
            GetCarRotations();
            // Cancel out carX and carZ movement, to stabilize the camera,
            // Leaving only the Y axis for moving upwards and downwards.
            transform.eulerAngles = new Vector3(carX - carX, carY, carZ - carZ);
        }

       // Get EulerAngle Rotations
        private void GetCarRotations()
        {
            carX = playerCar.transform.eulerAngles.x;
            carY = playerCar.transform.eulerAngles.y;
            carZ = playerCar.transform.eulerAngles.z;
        }
    }
}
