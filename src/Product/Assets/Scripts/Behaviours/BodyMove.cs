using AL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class BodyMove : MonoBehaviour {
    public RobotCoordinator robCord;

    void Update () {
        Vector3 headPos = InputTracking.GetLocalPosition(VRNode.Head);
        Vector3 leftHandPos = InputTracking.GetLocalPosition(VRNode.LeftHand) - headPos;
        Vector3 rightHandPos = InputTracking.GetLocalPosition(VRNode.RightHand) - headPos;
        float headAngle = InputTracking.GetLocalRotation(VRNode.Head).eulerAngles.y;
        float leftHandAngle = Mathf.Rad2Deg * Mathf.Atan2(leftHandPos.x, leftHandPos.z);
        float rightHandAngle = Mathf.Rad2Deg * Mathf.Atan2(rightHandPos.x, rightHandPos.z);

        if (headAngle > 180) {
            headAngle -= 360;
        }
        float offLeft = angleDiff(leftHandAngle, headAngle);
        float offRight = angleDiff(rightHandAngle, headAngle);

        float bodyAngle = headAngle + (offLeft + offRight) / 2;

        transform.position = headPos;
        transform.Rotate(Vector3.up * (bodyAngle - transform.eulerAngles.y));

        robCord.Theta = normalizedRadian(bodyAngle);
    }

    float angleDiff(float a1, float a2) {
        float diff = a1 - a2;
        if (diff > 180) {
            return diff - 360;
        }
        if (diff < -180) {
            return diff + 360;
        }
        return diff;
    }

    float normalizedRadian(float degree)
    {
        
        //degree = mod(degree, 360);
        if (degree > 180)
        {
            degree -= 360;
        }
        
        return Mathf.Deg2Rad * degree;
    }

    float mod(float x, int m)
    {
        return (x % m + m) % m;
    }
}
