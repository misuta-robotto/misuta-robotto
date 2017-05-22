using AL;
using System.Threading;
using UnityEngine;
using System;

/*
 * The intention of this class is to keep all the previus values sent to the robot for easier axess as well
 * as only having one connection with the robot. Many ALMotionProxys -> bad performance.
 */
public class RobotCoordinator : MonoBehaviour {
    public Calibration calibration;

    private const float SPEED_FRACTION = 1;
    private const float RESET_SPEED_FRACTION = 0.5f;
    private const float INITIAL_SHOULDER_PITCH = Mathf.PI / 2;
    private const float INITIAL_JOINT_ANGLE = 0;
    private const float WRIST_ANGLE = 0;

    private string[] pitchJoint = new string[] { "HeadPitch" };
    private string[] yawJoint = new string[] { "HeadYaw" };
    private string[] leftShoulderJoints = new string[] { "LShoulderPitch", "LShoulderRoll" };
    private string[] leftElbowJoints = new string[] { "LElbowYaw", "LElbowRoll" };
    private string[] rightShoulderJoints = new string[] { "RShoulderPitch", "RShoulderRoll" };
    private string[] rightElbowJoints = new string[] { "RElbowYaw", "RElbowRoll" };
    private string[] allJoints = new string[]
    {
        "HeadPitch",
        "HeadYaw",
        "LShoulderPitch",
        "LShoulderRoll",
        "LElbowYaw",
        "LElbowRoll",
        "RShoulderPitch",
        "RShoulderRoll",
        "RElbowYaw",
        "RElbowRoll",
        "RWristYaw",
        "LWristYaw"
    };
    private float leftShoulderPitch = Mathf.PI / 2;
    private float leftShoulderRoll = 0;
    private float leftElbowYaw = 0;
    private float leftElbowRoll = 0;
    private float rightShoulderPitch = Mathf.PI / 2;
    private float rightShoulderRoll = 0;
    private float rightElbowYaw = 0;
    private float rightElbowRoll = 0;
    private float headPitch = 0;
    private float headYaw = 0;
    private float rawHeadYaw = 0;
    private float x = 0;
    private float y = 0;
    private float theta = 0;
    private float currentTheta = 0;
    private float desiredTheta = 0;

    private bool isRunning = true;
    private bool isUpdating = false;

    // Setters for different Robot variables
    public float[] LeftShoulder {
        set {
            leftShoulderPitch = value[0];
            leftShoulderRoll = value[1];
        }
    }

    public float[] LeftElbow {
        set {
            leftElbowYaw = value[0];
            leftElbowRoll = value[1];
        }
    }

    public float[] RightShoulder {
        set {
            rightShoulderPitch = value[0];
            rightShoulderRoll = value[1];
        }
    }

    public float[] RightElbow {
        set {
            rightElbowYaw = value[0];
            rightElbowRoll = value[1];
        }
    }

    public float HeadPitch {
        set {
            headPitch = value;
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
            //Debug.Log(x + y+ theta);
        }
    }

    public float Theta
    {
        set
        {
            theta = value;
        }
    }

	public float DesiredTheta
	{
		set
		{
			desiredTheta = value;
            UpdateJaw();
        }
	}

    void Start () {
        calibration.ToggleMode += SetEnabled;
        new Thread(new ThreadStart(ThreadedLoop)).Start();
    }

    void SetEnabled(bool enabled)
    {
        isUpdating = enabled;
    }

    private void OnDisable()
    {
        isRunning = false;
    }

    private void UpdateJaw()
    {
        headYaw = rawHeadYaw - desiredTheta; // TODO: Consider using currentTheta or some mix between these two.
    }

	private void UpdateCurrentPosition(ALMotionProxy motionProxy)
	{
		float[] positions = motionProxy.GetRobotPosition (true);
		currentTheta = positions[2];
	}

    private float CalculateThetaAdaptionVelocity()
    {
        float baseVelocity = 0;
        if (desiredTheta < currentTheta)
        {
            baseVelocity = -1;
        }
        else if (desiredTheta > currentTheta)
        {
            baseVelocity = 1;
        }

        float diff = Mathf.Abs(desiredTheta - currentTheta);
        return baseVelocity * diff;
    }

    private void ThreadedLoop()
    {
        ALMotionProxy motionProxy = new ALMotionProxy(RobotConfiguration.ADRESS, RobotConfiguration.PORT);
        motionProxy.MoveInit();

        while (isRunning)
        {
            if (isUpdating)
            {
                motionProxy.SetAngles(allJoints, new float[] {
                    headPitch,
                    -headYaw,
                    leftShoulderPitch,
                    leftShoulderRoll,
                    leftElbowYaw,
                    leftElbowRoll,
                    rightShoulderPitch,
                    rightShoulderRoll,
                    rightElbowYaw,
                    rightElbowRoll,
                    WRIST_ANGLE,
                    WRIST_ANGLE
                }, SPEED_FRACTION);
                UpdateCurrentPosition(motionProxy);
                float thetaVelocity = CalculateThetaAdaptionVelocity();
                Debug.Log("currentTheta: " + currentTheta);
                Debug.Log("desiredTheta: " + desiredTheta);
                Debug.Log("thetaVelocity: " + thetaVelocity);
                motionProxy.Move(x, y, thetaVelocity);
            }
            else
            {
                motionProxy.SetAngles(allJoints, new float[] {
                    INITIAL_JOINT_ANGLE,
                    INITIAL_JOINT_ANGLE,
                    INITIAL_SHOULDER_PITCH,
                    INITIAL_JOINT_ANGLE,
                    INITIAL_JOINT_ANGLE,
                    INITIAL_JOINT_ANGLE,
                    INITIAL_SHOULDER_PITCH,
                    INITIAL_JOINT_ANGLE,
                    INITIAL_JOINT_ANGLE,
                    INITIAL_JOINT_ANGLE,
                    INITIAL_JOINT_ANGLE,
                    INITIAL_JOINT_ANGLE
                }, RESET_SPEED_FRACTION);
                motionProxy.Move(0, 0, 0);
            }
        }
    }
}
