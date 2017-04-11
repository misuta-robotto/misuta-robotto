using AL;
using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.VR;

public class RobotCoordinator : MonoBehaviour {
    private ALMotionProxy motionProxy;

    private const float SPEED_FRACTION = 1;
    private string[] pitchJoint = new string[] { "HeadPitch" };
    private string[] yawJoint = new string[] { "HeadYaw" };
    private float headPitch = 0;
    private float headYaw = 0;
    private float x = 0;
    private float y = 0;
    private float theta = 0;

    public float HeadPitch {
        set {
            headPitch = value;
            motionProxy.SetAngles(pitchJoint, new float[]{ headPitch }, SPEED_FRACTION);
        }
    }

    public float HeadYaw {
        set {
            headYaw = value - theta;
            motionProxy.SetAngles(yawJoint, new float[]{ headYaw }, SPEED_FRACTION);
        }
    }

    public float[] PadValues {
        set {
            x = value[0];
            y = value[1];
            theta = value[2];
            motionProxy.Move(x, y, theta);
        }
    }

    public float Theta
    {
        set
        {
            theta = value;
            motionProxy.MoveToAsync(x, y, theta);
        }
    }

    void Start () {
        motionProxy = new ALMotionProxy("127.0.0.1", 1234);
        motionProxy.MoveInit();
    }
}
