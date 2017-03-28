using AL;
using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmMove : MonoBehaviour {
    private string[] leftShoulderJoints = new string[] { "LShoulderPitch", "LShoulderRoll" };
    private string[] leftElbowJoints = new string[] { "LElbowYaw", "LElbowRoll" };
    private const float SPEED_FRACTION = 1;

    public Transform leftShoulder;
    public Transform leftUpperArm;
    public Transform leftForearm;
    public Transform leftWrist;
    private ALMotionProxy motionProxy;

    void Start () {
        motionProxy = new ALMotionProxy("127.0.0.1", 28759);
    }
	
	void Update () {
        float[] leftShoulderPitchAndRoll = ArmTranslation.TranslateShoulderPitchAndRoll(leftShoulder, leftUpperArm.position, leftForearm.position);
        float[] leftElbowYawAndRoll = ArmTranslation.TranslateElbowYawAndRoll(leftUpperArm, leftForearm.position, leftWrist.position);
        //double leftElbowRoll = ArmTranslation.TranslateElbowRoll();
        //Debug.Log("Angle: " + roll * Mathf.Rad2Deg);

        motionProxy.SetAngles(leftShoulderJoints, leftShoulderPitchAndRoll, SPEED_FRACTION);
        //Debug.Log("elbow roll: " + leftElbowYawAndRoll[1] * Mathf.Rad2Deg + ". yaw: " + leftElbowYawAndRoll[0] * Mathf.Rad2Deg);
        motionProxy.SetAngles(leftElbowJoints, leftElbowYawAndRoll, SPEED_FRACTION);
    }
}
