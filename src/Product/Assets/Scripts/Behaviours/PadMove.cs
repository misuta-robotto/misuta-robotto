using AL;
using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.VR;

public class PadMove : MonoBehaviour {
    public RobotCoordinator robCord {
        get; set;
    }
    private float x = 0;
    private float y = 0;
    private float theta = 0;

    //private ALMotionProxy motionProxy;
    public SteamVR_TrackedObject trackedObjLeft;

    // Add controllers
    private SteamVR_Controller.Device ControllerLeft {
	    get { return SteamVR_Controller.Input((int)trackedObjLeft.index); }
    }

    public SteamVR_TrackedObject trackedObjRight;
    private SteamVR_Controller.Device ControllerRight {
        get { return SteamVR_Controller.Input((int)trackedObjRight.index); }
    }

    void Update () {
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

        robCord.PadValues = new float[] { x, y, theta };
    }
}
