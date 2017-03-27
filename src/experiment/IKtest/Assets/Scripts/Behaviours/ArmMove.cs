using AL;
using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmMove : MonoBehaviour {
    private string[] jointNames = new string[] { "LShoulderPitch", "LShoulderRoll" /*, "LElbowRoll" */};
    private const float SPEED_FRACTION = 1;

    public Transform leftShoulder;
    public Transform leftUpperArm;
    public Transform leftForearm;
    public Transform leftWrist;
    private ALMotionProxy motionProxy;

    void Start () {
        motionProxy = new ALMotionProxy("127.0.0.1", 27977);
    }
	
	void Update () {
        float[] leftShoulderPitchAndRoll = ArmTranslation.TranslateShoulderPitchAndRoll(leftShoulder, leftUpperArm.position, leftForearm.position);
        //double leftElbowRoll = ArmTranslation.TranslateElbowRoll();
        //Debug.Log("Angle: " + roll * Mathf.Rad2Deg);

        motionProxy.SetAngles(jointNames, leftShoulderPitchAndRoll, SPEED_FRACTION);
	}
}
