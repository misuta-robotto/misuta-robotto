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
    private float rawHeadYaw = 0;
    private float x = 0;
    private float y = 0;
    private float theta = 0;
    private float lastTheta = 0;
    private float desiredTheta = 0;

    public float HeadPitch {
        set {
            headPitch = value;
            motionProxy.SetAngles(pitchJoint, new float[]{ headPitch }, SPEED_FRACTION);
        }
    }

    public float HeadYaw {
        set {
            rawHeadYaw = value;
            UpdateJaw();
        }
    }

    public float[] PadValues {
        set {
            //x = value[0];
            //y = value[1];
            //theta = value[2];
            //motionProxy.Move(x, y, theta);
        }
    }

    // TODO: Maybe rename to Rotation or something
    public float Theta
    {
        set
        {
            //lastTheta = theta;
            theta = value;// - theta;
            UpdateTheta();
            UpdateJaw();
        }
    }

    void Start () {
        motionProxy = new ALMotionProxy("127.0.0.1", 5890);
        motionProxy.MoveInit();
    }

    private void UpdateJaw()
    {
        headYaw = rawHeadYaw - theta;
        motionProxy.SetAngles(yawJoint, new float[] { -headYaw }, SPEED_FRACTION);
    }

    private void UpdateTheta()
    {

        Debug.Log("Body angle: " + theta + ", Unity head angle: " + rawHeadYaw + ", Head angle sent to Pepper: " + -headYaw + ", x/y: " + x + y);
        if (Mathf.Abs(theta) > 0.3)
        {
            motionProxy.MoveTo(x, y, theta);
        }
    }
}
