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
using UnityEngine.UI;

/*
 * Manages VR HUD and settings
 */
public class VRHud : MonoBehaviour {
    public Text heightText;
    public Calibration calibration;

    public Canvas hud;
    public Canvas crosshairTarget;
    public SteamVR_Camera cam;

    void Start() {
        calibration.ToggleMode += SetEnabled;
        hud.enabled = true;
        crosshairTarget.enabled = true;
        cam.camera.farClipPlane = 5020f;
    }

    private void SetEnabled(bool b) {
        // Enable/disable HUD Canvases
        hud.enabled = !b;
        crosshairTarget.enabled = !b;

        // Change camera render distance
        if (b) {
            cam.camera.farClipPlane = 500f;
        }
        else {
            cam.camera.farClipPlane = 5020f;
        }
    }

    // Update local text and convert m to cm
    public void UpdateHeight(float height)
    {
        heightText.text = Mathf.Round(height * 100f).ToString();
    }
}
