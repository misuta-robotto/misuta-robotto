/*
Copyright (c) 2017, Misuta Robotto Group

The contents of this file are subject to the Common Public Attribution License Version 1.0 (the “License”); 
you may not use this file except in compliance with the License. You may obtain a copy of the License at

    https://github.com/Emiluren/misuta-robotto/blob/master/LICENSE.md
    
The License is based on the Mozilla Public License Version 1.1 but Sections 14 and 15 have been added to cover
use of software over a computer network and provide for limited attribution for the Original Developer. In 
addition, Exhibit A has been modified to be consistent with Exhibit B.

Software distributed under the License is distributed on an “AS IS” basis, WITHOUT WARRANTY OF ANY KIND, 
either express or implied. See the License  for the specific language governing rights and limitations 
under the License.

The Original Code is Misuta Robotto.

The Initial Developer of the Original Code is Misuta Robotto Group. 
All portions of the code written by Misuta Robotto Group are Copyright (c) 2017. All Rights Reserved.

Misuta Robotto Group includes Robin Christensen, Jacob Lundberg, Ylva Lundegård, Emil Segerbäck,
Patrik Sletmo, Teo Tiefenbacher, Jon Vik and David Wajngot.
*/

using UnityEngine;
using UnityEngine.VR;

public class BodyMove : MonoBehaviour {
    public RobotCoordinator robCord;
    public Calibration calibration;

    void Start () {
        calibration.ToggleMode += SetEnabled;
        enabled = false;
    }

    void SetEnabled(bool b) {
        enabled = b;
    }

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

        // Calculate body angle
        float offLeft = angleDiff(leftHandAngle, headAngle);
        float offRight = angleDiff(rightHandAngle, headAngle);
        float bodyAngle = headAngle + ((offLeft + offRight) / 2);

        transform.position = headPos;
        transform.Rotate(Vector3.up * (bodyAngle - transform.eulerAngles.y));

        // Send data to RobotCoordinator
        robCord.DesiredPositionX = headPos.x;
        robCord.DesiredPositionZ = headPos.z;
        robCord.DesiredTheta = normalizedRadian(-bodyAngle);
    }

    private float angleDiff(float a1, float a2) {
        float diff = a1 - a2;
        if (diff > 180) {
            return diff - 360;
        }
        if (diff < -180) {
            return diff + 360;
        }
        return diff;
    }

    private float normalizedRadian(float degree)
    {
        if (degree > 180)
        {
            degree -= 360;
        }

        return Mathf.Deg2Rad * degree;
    }

    float mod(float x, int m)
    {
        return ((x % m) + m) % m;
    }
}
