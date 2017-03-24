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
     private SteamVR_TrackedObject trackedObj;
     private SteamVR_Controller.Device Controller
     {
	      get { return SteamVR_Controller.Input((int)trackedObj.index); }
     }

     void Start () {
        motionProxy = new ALMotionProxy("127.0.0.1", 13413);
        Debug.log("Started ")
        motionProxy.MoveInit();
     }

     void Awake () {
     	  trackedObj = GetComponent<SteamVR_TrackedObject>();
     }

     void Update () {
       if (Controller.GetAxis() != Vector2.zero) {
         motionProxy.Move(0, Controller.GetAxis().y, Controller.GetAxis().x);
         Debug.Log(gameObject.name + Controller.GetAxis());
       }
     }
}
