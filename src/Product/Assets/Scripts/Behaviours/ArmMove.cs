using AL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class ArmMove : MonoBehaviour {
    private ALMotionProxy motionProxy;

    // Use this for initialization
    void Start ()
    {
        //motionProxy = new ALMotionProxy("127.0.0.1", 23342);
    }

    // Update is called once per frame
    void Update() {
        var rightHandPos = InputTracking.GetLocalPosition(VRNode.RightHand);
        var leftHandPos = InputTracking.GetLocalPosition(VRNode.LeftHand);

        var rightHandOrientation = InputTracking.GetLocalRotation(VRNode.RightHand);
        var leftHandOrientation = InputTracking.GetLocalRotation(VRNode.LeftHand);

        string[] arms = { "LArm", "RArm" };
        var leftEuler = leftHandOrientation.eulerAngles;
        var rightEuler = rightHandOrientation.eulerAngles;
        float[] pos_data = {
            leftHandPos.x, leftHandPos.y, leftHandPos.z,
            leftEuler.x, leftEuler.y, leftEuler.z,
            rightHandPos.x, rightHandPos.y, rightHandPos.z,
            rightEuler.x, rightEuler.y, rightEuler.z,
        };
        //motionProxy.SetPositions(arms, ALMotionProxy.FRAME_ROBOT, pos_data, 1,
        //    ALMotionProxy.AXIS_MASK_POSITION | ALMotionProxy.AXIS_MASK_ROTATION);
    }
}
