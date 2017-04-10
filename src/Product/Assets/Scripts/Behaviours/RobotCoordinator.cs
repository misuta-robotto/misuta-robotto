using AL;
using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.VR;

public class RobotCoordinator : MonoBehaviour {
    private ALMotionProxy motionProxy;
    private float headPitch = 0;
    private float headYaw = 0;
    

    void Start () {
        motionProxy = new ALMotionProxy("127.0.0.1", 1234);


    }






}
