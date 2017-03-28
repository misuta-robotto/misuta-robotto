using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class BodyMove : MonoBehaviour {
	void Update () {
        Vector3 headPos = InputTracking.GetLocalPosition(VRNode.Head);
        Vector3 leftHandPos = InputTracking.GetLocalPosition(VRNode.LeftHand);
        Vector3 rightHandPos = InputTracking.GetLocalPosition(VRNode.RightHand);
        float headAngle = InputTracking.GetLocalRotation(VRNode.Head).eulerAngles.y;
        float leftHandAngle = Mathf.Rad2Deg * Mathf.Atan2(leftHandPos.x, leftHandPos.z);
        float rightHandAngle = Mathf.Rad2Deg * Mathf.Atan2(rightHandPos.x, rightHandPos.z);
        float bodyAngle = headAngle + (leftHandAngle + rightHandAngle) / 3.0f;
        Debug.Log("head: " + headAngle + ". left: " + leftHandAngle + ". right: " + rightHandAngle);

        transform.position = headPos;
        transform.Rotate(Vector3.up * (bodyAngle - transform.eulerAngles.y));
    }
}
