using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BLACK.Core
{
    public class CameraAngleCorrection : MonoBehaviour
    {

        // Obtain GameObjects for Transform and Translate Vales
        public GameObject playerCar;
        public GameObject playerCamera;

        // Variable to Isolate X Axis Rotation of Car
        public float carRotationX;
        [SerializeField]
        private float cameraUpThresh = 10f;
        [SerializeField]
        private float cameraDownThresh = 10f;
        void Update()
        {

            // TODO: Smooth out Camera Movement between transforms
            // TODO: Fix bug with carRotation <= -15 interacting weirdly with the camera resetting to original
            // Get the X Axis Rotation
            carRotationX = playerCar.transform.eulerAngles.x;
            print(carRotationX);
            // If carRotationX is greater than an angle of 15, camera will shift to help see down hills.        
            if (carRotationX >= cameraUpThresh && carRotationX<180)
            {
                playerCamera.transform.localPosition = new Vector3(0, 5.8f, -26f);
                playerCamera.transform.localEulerAngles = new Vector3(8.9f, 0, 0);
            }
            // Else, if carRotationX is less than an angle of -15, camera will shift to help see up hills.
            else if (carRotationX <=360-cameraDownThresh&&carRotationX>=180)
            {
                playerCamera.transform.localPosition = new Vector3(0, 7.8f, -23.5f);
                playerCamera.transform.localEulerAngles = new Vector3(15, 0, 0);
            }
            // Else, if carRotation evens back out to around 0-1, camera will shift back to original location.
            else //if (carRotationX > -1 || carRotationX < 1f)
            {
                playerCamera.transform.localPosition = new Vector3(0, 1, -24f);
                playerCamera.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
        }
    }


}