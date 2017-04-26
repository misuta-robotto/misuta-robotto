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

    public RobotCoordinator robCord;
    public Calibration calibration;

    void Start() {
        calibration.ToggleMode += SetEnabled;
        enabled = false;
    }

    void SetEnabled(bool b) {
        enabled = b;
    }

	void Update () {
        robCord.LeftShoulder = ArmTranslation.TranslateLeftShoulderPitchAndRoll(leftShoulder, leftUpperArm.position, leftForearm.position);
        robCord.LeftElbow = ArmTranslation.TranslateLeftElbowYawAndRoll(leftUpperArm, leftForearm.position, leftWrist.position);
        robCord.RightShoulder = ArmTranslation.TranslateRightShoulderPitchAndRoll(rightShoulder, rightUpperArm.position, rightForearm.position);
        robCord.RightElbow = ArmTranslation.TranslateRightElbowYawAndRoll(rightUpperArm, rightForearm.position, rightWrist.position);
    }
}
