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
using UnityEngine.VR;
using UnityEngine.UI;
using MishutaRoboto;
/*
 * Calibration handles the calibration of the users 3D representation in Unity
 * so that its size is a good estimate of the users size. This helps to more
 * accuratly represent the users arm movements.  
 */

public class Calibration : MonoBehaviour
{
    public SteamVR_TrackedController leftController;
    public SteamVR_TrackedController rightController;
    public VRHud hud;

    public delegate void ToggleCalibrationMode(bool b);
    public event ToggleCalibrationMode ToggleMode;
    public bool calibrationMode;

    public Transform kyle;
    public float sizeRatio;
    public float userHeight; //m

    public Toggle toggle;
    public InputField manual_calibration_input;
    public Button manual_calibration_button;

    public float MIN_HEIGHT = 0.5f;
    public float MAX_HEIGHT = 2.5f;

    void Start()
    {
        toggle.isOn = true;
        userHeight = HeightTranslator.KYLE_HEIGHT;
        sizeRatio = 1f;

        // Uncomment to enable automatic calibration when testing
        //Invoke("ToggleManual", 3);
    }

    void ToggleManual()
    {
        ToggleMode(true);
    }
    
    /*
    When the script is enabled it subscribes itself to events for the controller
        actions TriggerClicked and MenuButtonClicked.
    */
    private void OnEnable()
    {
        leftController.TriggerClicked += HandleTriggerClicked;
        rightController.TriggerClicked += HandleTriggerClicked;
        leftController.MenuButtonClicked += HandleMenuButtonClicked;
        rightController.MenuButtonClicked += HandleMenuButtonClicked;
        toggle.onValueChanged.AddListener(delegate { UpdateCalibrationMode(); });
    }

    /*
    When the script is disabled it unsubscribes itself from the events.
    */
    private void OnDisable()
    {
        leftController.TriggerClicked -= HandleTriggerClicked;
        rightController.TriggerClicked -= HandleTriggerClicked;
        leftController.MenuButtonClicked -= HandleMenuButtonClicked;
        rightController.MenuButtonClicked -= HandleMenuButtonClicked;
    }

    /*
    Resizes the 3D representation of the user in Unity when both triggers are
    pressed and in system is in calibration mode.
    */
    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {
        if (leftController.triggerPressed && rightController.triggerPressed && toggle.isOn)
        {
            userHeight = HeightTranslator.CalculateHeight(InputTracking.GetLocalPosition(VRNode.Head));
            hud.UpdateHeight(userHeight);
            ResizeKyle();
        }
    }


    public void ResizeKyle()
    {
        sizeRatio = HeightTranslator.CalculateSizeRatio(userHeight);
        kyle.localScale = new Vector3(sizeRatio, sizeRatio, sizeRatio);
    }

    /*
    Toggels calibration mode and signals the toggle to all subscribers of the
    ToggleMode event when the menubutton is pressed. The subscribers use this
    for example to keep the robot still or render different objects of the scene
    during calibration.
    */
    private void HandleMenuButtonClicked(object sender, ClickedEventArgs e)
    {
        toggle.isOn = !toggle.isOn;
    }

    private void UpdateCalibrationMode()
    {
        ToggleMode(!toggle.isOn);
    }

    public void ManualCalibration()
    {
        string height = manual_calibration_input.text.ToString();
        float heightFloat;
        if (float.TryParse(height, out heightFloat))
        {
            
            userHeight = heightFloat.Clamp(MIN_HEIGHT, MAX_HEIGHT);
            if (toggle.isOn)
            {
                hud.UpdateHeight(userHeight);
                ResizeKyle();
                Debug.Log("(SUCCESS manual) height: " + userHeight);
            }
        }

    }
}

