using AL;
using System.Threading;
using UnityEngine;

/*
 * The intention of this class is to keep all the previus values sent to the robot for easier axess as well
 * as only having one connection with the robot. Many ALMotionProxys -> bad performance.
 */
public class RobotCoordinator : MonoBehaviour {
    public Calibration calibration;

    private const float SPEED_FRACTION = 1;
    private const float RESET_SPEED_FRACTION = 0.5f;
    private string[] pitchJoint = { "HeadPitch" };
    private string[] yawJoint = { "HeadYaw" };
    private string[] leftShoulderJoints = { "LShoulderPitch", "LShoulderRoll" };
    private string[] leftElbowJoints = { "LElbowYaw", "LElbowRoll" };
    private string[] rightShoulderJoints = { "RShoulderPitch", "RShoulderRoll" };
    private string[] rightElbowJoints = { "RElbowYaw", "RElbowRoll" };
    private static readonly string[] ALL_JOINTS =
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
        "LWristYaw",
        "RHand",
        "LHand"
    };
    private static readonly float[] DEFAULT_JOINT_VALUES =
        { 0, 0, Mathf.PI / 2, 0, 0, 0, Mathf.PI / 2, 0, 0, 0, 0, 0, 1, 1 };
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
    private float lastTheta = 0;
    private float desiredTheta = 0;
    public bool LeftHandOpen { get; set; }
    public bool RightHandOpen { get; set; }

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
            UpdateTheta();
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
        //headYaw = rawHeadYaw - theta; removes for KVIT
        //motionProxy.SetAngles(yawJoint, new float[] { -rawHeadYaw }, SPEED_FRACTION);
    }

    //This feature isn't good enough for KVIT demo yet
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

    private void ThreadedLoop()
    {
        ALMotionProxy motionProxy = new ALMotionProxy(RobotConfiguration.ADRESS, RobotConfiguration.PORT);
        motionProxy.MoveInit();

        while (isRunning)
        {
            if (isUpdating) {
                float rhand = RightHandOpen ? 1 : 0;
                float lhand = LeftHandOpen ? 1 : 0;
                motionProxy.SetAngles(ALL_JOINTS, new float[] { headPitch, -rawHeadYaw, leftShoulderPitch, leftShoulderRoll, leftElbowYaw, leftElbowRoll, rightShoulderPitch, rightShoulderRoll, rightElbowYaw, rightElbowRoll, 0, 0, rhand, lhand }, SPEED_FRACTION);
                motionProxy.Move(x, y, theta);
            }
            else
            {
                motionProxy.SetAngles(ALL_JOINTS, DEFAULT_JOINT_VALUES, RESET_SPEED_FRACTION);
                motionProxy.Move(0, 0, 0);
            }
        }
    }
}
