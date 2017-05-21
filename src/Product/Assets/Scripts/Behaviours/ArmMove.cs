using Assets;
using UnityEngine;

public class ArmMove : MonoBehaviour {
    public SteamVR_TrackedController leftController;
    public SteamVR_TrackedController rightController;

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
        robCord.LeftShoulder = ArmTranslation.TranslateShoulderPitchAndRoll(leftShoulder, leftUpperArm.position, leftForearm.position, Side.Left);
        robCord.LeftElbow = ArmTranslation.TranslateElbowYawAndRoll(leftUpperArm, leftForearm.position, leftWrist.position, Side.Left);
        robCord.RightShoulder = ArmTranslation.TranslateShoulderPitchAndRoll(rightShoulder, rightUpperArm.position, rightForearm.position, Side.Right);
        robCord.RightElbow = ArmTranslation.TranslateElbowYawAndRoll(rightUpperArm, rightForearm.position, rightWrist.position, Side.Right);

        robCord.LeftHandOpen = !leftController.triggerPressed;
        robCord.RightHandOpen = !rightController.triggerPressed;
    }
}
