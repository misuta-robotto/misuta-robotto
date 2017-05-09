using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmMove : MonoBehaviour {
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

    // Translate arm data and send to RobotCoordinator
	void Update () {
        robCord.LeftShoulder = ArmTranslation.TranslateLeftShoulderPitchAndRoll(leftShoulder, leftUpperArm.position, leftForearm.position);
        robCord.LeftElbow = ArmTranslation.TranslateLeftElbowYawAndRoll(leftUpperArm, leftForearm.position, leftWrist.position);
        robCord.RightShoulder = ArmTranslation.TranslateRightShoulderPitchAndRoll(rightShoulder, rightUpperArm.position, rightForearm.position);
        robCord.RightElbow = ArmTranslation.TranslateRightElbowYawAndRoll(rightUpperArm, rightForearm.position, rightWrist.position);
    }
}
