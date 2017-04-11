using AL;
using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.VR;

public class RobotCoordinator : MonoBehaviour {
    private const float SPEED_FRACTION = 1;
    private string[] pitchJoint = new string[] { "HeadPitch" };
    private string[] yawJoint = new string[] { "HeadYaw" };
    private ALMotionProxy motionProxy;
    private float headPitch = 0;
    private float headYaw = 0;

    public float HeadPitch {
        set {
            headPitch = value;
            motionProxy.SetAngles(pitchJoint, new float[]{ headPitch }, SPEED_FRACTION);
        }
    }

    public float HeadYaw {
        set {
            headYaw = value;
            motionProxy.SetAngles(yawJoint, new float[]{ headYaw }, SPEED_FRACTION);
        }
    }

    void Start () {
        motionProxy = new ALMotionProxy("127.0.0.1", 1234);
    }
}
