using AL;
using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.VR;

public class PadMoveLeft : MonoBehaviour {
     private ALMotionProxy motionProxy;
     private SteamVR_TrackedObject trackedObj;
     private SteamVR_Controller.Device Controller
     {
	      get { return SteamVR_Controller.Input((int)trackedObj.index); }
     }

     void Start () {
        motionProxy = new ALMotionProxy("127.0.0.1", 2104);
        Debug.Log("Started ");
        //motionProxy.MoveInit();
     }

     void Awake () {
     	  trackedObj = GetComponent<SteamVR_TrackedObject>();
     }

     void Update () {
        //if (Controller.GetAxis().x, 0, 0) {
        float x = 0;
        float y = 0;

        if (System.Math.Abs(Controller.GetAxis().y) > 0.2)
        {
            x = Controller.GetAxis().y;
        }
        /* SIDLEDSFÖRFLYTTNING
        if (System.Math.Abs(Controller.GetAxis().x) > 0.2)
        {
            y = Controller.GetAxis().x;
        }*/

        motionProxy.Move(x, 0, 0);

         //Debug.Log(gameObject.name + Controller.GetAxis());
    }
     
}
