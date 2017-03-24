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
     //private SteamVR_TrackedObject trackedObj;
     //private SteamVR_Controller.Device Controller
     /*{
	      get { return SteamVR_Controller.Input((int)trackedObj.index); }
     }*/

     void Start () {
        motionProxy = new ALMotionProxy("127.0.0.1", 2104);
        Debug.Log("Started ");
        motionProxy.MoveInit();
     }

     /*void Awake () {
     	trackedObj = GetComponent<SteamVR_TrackedObject>();
     }*/

     void Update () {
        
     }
}
