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
        robCord.LeftWristYaw = ArmTranslation.TranslateWristYaw(leftWrist, Side.Left);
        robCord.RightWristYaw = ArmTranslation.TranslateWristYaw(rightWrist, Side.Right);

        robCord.LeftHandClosedAmount = 1 - leftController.controllerState.rAxis1.x;
        robCord.RightHandClosedAmount = 1 - rightController.controllerState.rAxis1.x;
    }
}
