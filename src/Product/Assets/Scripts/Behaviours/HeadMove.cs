using AL;
using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.VR;

public class HeadMove : MonoBehaviour {
    public RobotCoordinator robCord;

    private HeadTranslator headTranslator;

    // Use this for initialization
    void Start () {
        headTranslator = new HeadTranslator();
    }

	// Update is called once per frame
	void Update () {
        float pitch = InputTracking.GetLocalRotation(VRNode.Head).eulerAngles.x; //get Vive pitch
        float yaw = InputTracking.GetLocalRotation(VRNode.Head).eulerAngles.y; //get Vive yaw

        robCord.HeadPitch = headTranslator.TranslatePitch(pitch);
        robCord.HeadYaw = -headTranslator.TranslateYaw(yaw);
    }
}
