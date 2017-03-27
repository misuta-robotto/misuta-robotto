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
    private SteamVR_Controller.Device ControllerLeft {
	    get { return SteamVR_Controller.Input((int)trackedObjLeft.index); }
    }

    public SteamVR_TrackedObject trackedObjRight;
    private SteamVR_Controller.Device ControllerRight {
        get { return SteamVR_Controller.Input((int)trackedObjRight.index); }
    }

    void Start () {
        motionProxy = new ALMotionProxy("127.0.0.1", 1743);
        Debug.Log("Started ");
        motionProxy.MoveInit();
     }

     void Update () {
        // Ej testat!
        float x = 0;
        float y = 0;
        float theta = 0;

        // ControllerLeft - Förflyttning framåt och bakåt
        if (System.Math.Abs(ControllerLeft.GetAxis().y) > 0.2)
        {
            x = ControllerLeft.GetAxis().y;
        }

        // ControllerLeft - Sidledsförflyttning
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
        //Debug.Log(gameObject.name + Controller.GetAxis());
    }
}
