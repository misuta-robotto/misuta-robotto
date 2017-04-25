using AL;
using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmMove : MonoBehaviour {
    private string[] leftShoulderJoints = new string[] { "LShoulderPitch", "LShoulderRoll" };
    private string[] leftElbowJoints = new string[] { "LElbowYaw", "LElbowRoll" };
    private string[] rightShoulderJoints = new string[] { "RShoulderPitch", "RShoulderRoll" };
    private string[] rightElbowJoints = new string[] { "RElbowYaw", "RElbowRoll" };
    private const float SPEED_FRACTION = 1;

    public Transform leftShoulder;
    public Transform leftUpperArm;
    public Transform leftForearm;
    public Transform leftWrist;

    public Transform rightShoulder;
    public Transform rightUpperArm;
    public Transform rightForearm;
    public Transform rightWrist;

    private ALMotionProxy motionProxy;

    void Start () {
        motionProxy = new ALMotionProxy(RobotConfiguration.ADRESS, RobotConfiguration.PORT);
    }
	
	void Update () {
        float[] leftShoulderPitchAndRoll = ArmTranslation.TranslateLeftShoulderPitchAndRoll(leftShoulder, leftUpperArm.position, leftForearm.position);
        float[] leftElbowYawAndRoll = ArmTranslation.TranslateLeftElbowYawAndRoll(leftUpperArm, leftForearm.position, leftWrist.position);
        float[] rightShoulderPitchAndRoll = ArmTranslation.TranslateRightShoulderPitchAndRoll(rightShoulder, rightUpperArm.position, rightForearm.position);
        float[] rightElbowYawAndRoll = ArmTranslation.TranslateRightElbowYawAndRoll(rightUpperArm, rightForearm.position, rightWrist.position);

        motionProxy.SetAngles(leftShoulderJoints, leftShoulderPitchAndRoll, SPEED_FRACTION);
        motionProxy.SetAngles(leftElbowJoints, leftElbowYawAndRoll, SPEED_FRACTION);
        motionProxy.SetAngles(rightShoulderJoints, rightShoulderPitchAndRoll, SPEED_FRACTION);
        motionProxy.SetAngles(rightElbowJoints, rightElbowYawAndRoll, SPEED_FRACTION);
    }
}
