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
    private string[] leftShoulderJoints = new string[] { "LShoulderPitch", "LShoulderRoll" };
    private string[] leftElbowJoints = new string[] { "LElbowYaw", "LElbowRoll" };
    private string[] rightShoulderJoints = new string[] { "RShoulderPitch", "RShoulderRoll" };
    private string[] rightElbowJoints = new string[] { "RElbowYaw", "RElbowRoll" };
    private float leftShoulderPitch = 0;
    private float leftShoulderRoll = 0;
    private float leftElbowYaw = 0;
    private float leftElbowRoll = 0;
    private float rightShoulderPitch = 0;
    private float rightShoulderRoll = 0;
    private float rightElbowYaw = 0;
    private float rightElbowRoll = 0;
    private float headPitch = 0;
    private float headYaw = 0;
    private float rawHeadYaw = 0;
    private float x = 0;
    private float y = 0;
    private float theta = 0;
    private float lastTheta = 0;
    private float desiredTheta = 0;

    
    public float[] LeftShoulder {
        set {
            leftShoulderPitch = value[0];
            leftShoulderRoll = value[1];
            motionProxy.SetAngles(leftShoulderJoints, new float[] { leftShoulderPitch, leftShoulderRoll  }, SPEED_FRACTION);
        }
    }
    
    public float[] LeftElbow {
        set {
            leftElbowYaw = value[0];
            leftElbowRoll = value[1];
            motionProxy.SetAngles(leftElbowJoints, new float[] { leftElbowYaw, leftElbowRoll }, SPEED_FRACTION);
        }
    }

    public float[] RightShoulder {
        set {
            rightShoulderPitch = value[0];
            rightShoulderRoll = value[1];
            motionProxy.SetAngles(rightShoulderJoints, new float[] { rightShoulderPitch, rightShoulderRoll }, SPEED_FRACTION);
        }
    }

    public float[] RightElbow {
        set {
            rightElbowYaw = value[0];
            rightElbowRoll = value[1];
            motionProxy.SetAngles(rightElbowJoints, new float[] { rightElbowYaw, rightElbowRoll }, SPEED_FRACTION);
        }
    }
    
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
            x = value[0];
            y = value[1];
            theta = value[2];
            motionProxy.Move(x, y, theta);
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
        motionProxy = new ALMotionProxy(RobotConfiguration.ADRESS, RobotConfiguration.PORT);
        motionProxy.MoveInit();
    }

    private void UpdateJaw()
    {
        //headYaw = rawHeadYaw - theta; removes for KVIT
        motionProxy.SetAngles(yawJoint, new float[] { -rawHeadYaw }, SPEED_FRACTION);
    }

    //This feature isn't good enough for KVIT demo
    private void UpdateTheta()
    {
        /*
        Debug.Log("Body angle: " + theta + ", Unity head angle: " + rawHeadYaw + ", Head angle sent to Pepper: " + -headYaw + ", x/y: " + x + y);
        if (Mathf.Abs(theta) > 0.3)
        {
            motionProxy.MoveTo(x, y, theta);
        }
        */
    }
}
