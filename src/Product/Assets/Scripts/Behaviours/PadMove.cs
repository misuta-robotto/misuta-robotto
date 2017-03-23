using AL;
using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.VR;

public class HeadMove : MonoBehaviour {
     private SteamVR_TrackedObject trackedObj;
     private SteamVR_Controller.Device Controller
     {
	      get { return SteamVR_Controller.Input((int)trackedObj.index); }
     }

     void Awake () {
     	  trackedObj = GetComponent<SteamVR_TrackedObject>();
     }

     void Update () {
       if (Controller.GetAxis() != Vector2.zero) {
         //Do shit
         Debug.Log(gameObject.name + Controller.GetAxis());
       }
     }
}
