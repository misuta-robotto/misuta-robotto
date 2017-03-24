using AL;
using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.VR;

public class PadMoveRight : MonoBehaviour {
     private ALMotionProxy motionProxy;
     private SteamVR_TrackedObject trackedObj;
     private SteamVR_Controller.Device Controller
     {
	    get { return SteamVR_Controller.Input((int)trackedObj.index); }
     }

     void Start () {
        motionProxy = new ALMotionProxy("127.0.0.1", 2104);
        Debug.Log("Started ");
        motionProxy.MoveInit();
     }

     void Awake () {
     	trackedObj = GetComponent<SteamVR_TrackedObject>();
     }

     void Update () {
        // Ej testat!
        float theta = 0;

        if (System.Math.Abs(Controller.GetAxis().y) > 0.2)
        {
            theta = -Controller.GetAxis().y;
        }

        motionProxy.Move(0, 0, theta);
        //Debug.Log(gameObject.name + Controller.GetAxis());
    }
}
