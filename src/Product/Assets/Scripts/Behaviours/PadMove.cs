using AL;
using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.VR;

public class PadMove : MonoBehaviour {
    private ALMotionProxy motionProxy;
    public SteamVR_TrackedObject trackedObjLeft;

    // Add controllers
    private SteamVR_Controller.Device ControllerLeft {
	    get { return SteamVR_Controller.Input((int)trackedObjLeft.index); }
    }

    public SteamVR_TrackedObject trackedObjRight;
    private SteamVR_Controller.Device ControllerRight {
        get { return SteamVR_Controller.Input((int)trackedObjRight.index); }
    }

    void Start () {
        motionProxy = new ALMotionProxy(RobotConfiguration.ADRESS, RobotConfiguration.PORT);
        motionProxy.MoveInit();
     }

     void Update () {
        float x = 0;
        float y = 0;
        float theta = 0;

        // ControllerLeft - Movement back and forth
        if (System.Math.Abs(ControllerLeft.GetAxis().y) > 0.2)
        {
            x = ControllerLeft.GetAxis().y;
        }

        // ControllerLeft - Movement left and right
        if (System.Math.Abs(ControllerLeft.GetAxis().x) > 0.2)
        {
            y = -ControllerLeft.GetAxis().x;
        }

        // ControllerRight - Rotation
        if (System.Math.Abs(ControllerRight.GetAxis().x) > 0.2)
        {
            theta = -ControllerRight.GetAxis().x;
        }

        motionProxy.Move(x, y, theta);
    }
}
