using AL;
using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.VR;

public class HeadMove : MonoBehaviour {
    private string[] jointNames = new string[] { "HeadYaw", "HeadPitch" };
    private const float SPEED_FRACTION = 1;

    private HeadTranslator headTranslator;
    private ALMotionProxy motionProxy;

    // Use this for initialization
    void Start () {
        headTranslator = new HeadTranslator();
        motionProxy = new ALMotionProxy("127.0.0.1", 33551);

        Debug.Log("Started");
    }
	
	// Update is called once per frame
	void Update () {
        float pitch = InputTracking.GetLocalRotation(VRNode.Head).eulerAngles.x; //get Vive pitch
        float yaw = InputTracking.GetLocalRotation(VRNode.Head).eulerAngles.y; //get Vive yaw

        pitch = headTranslator.TranslatePitch(pitch);
        yaw = headTranslator.TranslateYaw(yaw);
        
        motionProxy.SetAngles(jointNames, new float[] { -yaw, pitch }, SPEED_FRACTION); //Send angles to robot

        var rightHandPos = InputTracking.GetLocalPosition(VRNode.RightHand);
        var leftHandPos = InputTracking.GetLocalPosition(VRNode.LeftHand);

        var rightHandOrientation = InputTracking.GetLocalRotation(VRNode.RightHand);
        var leftHandOrientation = InputTracking.GetLocalRotation(VRNode.LeftHand);

        string[] arms = { "LArm"};
        var leftEuler = leftHandOrientation.eulerAngles;
        var rightEuler = rightHandOrientation.eulerAngles;
        float[] pos_data = {
            0, 0, 0.25f,
            0, 0, 0
            //rightHandPos.x, rightHandPos.y, rightHandPos.z,
            //rightEuler.x, rightEuler.y, rightEuler.z,
        };
        //motionProxy.SetPositions(arms, 2, pos_data, 1, 63);
    } 
}
