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

using System;
using UnityEngine;

public class PadMove : MonoBehaviour {
    public SteamVR_TrackedController leftController;
    public SteamVR_TrackedController rightController;
    public RobotCoordinator robCord;
    public Calibration calibration;

    private const float CONTROLLER_SENSITIVITY_LIMIT = 0.2f;
    
    private float x = 0;
    private float y = 0;
    private float theta = 0;

    void Start () {
        calibration.ToggleMode += SetEnabled;
        enabled = false;
    }

    void SetEnabled(bool b) {
        enabled = b;
    }

    void Update()
    {
        // ControllerLeft - Movement back and forth
        if (Math.Abs(leftController.controllerState.rAxis0.y) > CONTROLLER_SENSITIVITY_LIMIT)
        {
            x = leftController.controllerState.rAxis0.y;
        }
        else
        {
            x = 0;
        }

        // ControllerLeft - Movement left and right
        if (Math.Abs(leftController.controllerState.rAxis0.x) > CONTROLLER_SENSITIVITY_LIMIT)
        {
            y = -leftController.controllerState.rAxis0.x;
        }
        else
        {
            y = 0;
        }

        // ControllerRight - Rotation
        if (Math.Abs(rightController.controllerState.rAxis0.x) > CONTROLLER_SENSITIVITY_LIMIT)
        {
            theta = -rightController.controllerState.rAxis0.x;
        }
        else
        {
            theta = 0;
        }
            
        robCord.PadValues = new float[] { x, y, theta };
    } 

}
