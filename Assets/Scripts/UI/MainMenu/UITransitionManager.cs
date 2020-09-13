﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UITransitionManager : MonoBehaviour
{

    public CinemachineVirtualCamera currentCamera;


    public void Start()
    {
        currentCamera.Priority++;
    }


    public void UpdateCamera(CinemachineVirtualCamera target)
    {
        currentCamera.Priority--;

        currentCamera = target;

        currentCamera.Priority++;
    }
}
