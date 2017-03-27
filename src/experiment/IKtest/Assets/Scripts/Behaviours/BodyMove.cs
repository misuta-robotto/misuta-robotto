using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class BodyMove : MonoBehaviour {
	void Update () {
        Vector3 pos = InputTracking.GetLocalPosition(VRNode.Head);
        float bodyAngle = InputTracking.GetLocalRotation(VRNode.Head).eulerAngles.y;

        transform.position = pos;
        transform.Rotate(Vector3.up * (bodyAngle - transform.eulerAngles.y));
    }
}
