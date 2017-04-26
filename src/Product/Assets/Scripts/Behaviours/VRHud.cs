using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRHud : MonoBehaviour {
    public Text heightText;
    public Calibration calibration;
    public Canvas vrHUD;
    public Canvas crossHairTarget;
    public SteamVR_Camera cam;
    public Camera otherCam;

    void Start() {
        calibration.ToggleMode += SetEnabled;
        vrHUD.enabled = true;
        crossHairTarget.enabled = true;

        //If i understood source code correctly
        cam.Head.camera.farClipPlane = 5020f;

        //Otherwise this will probably do
        //otherCam.farClipPlane = 5020f;
    }

    private void SetEnabled(bool b) {
        //Enable/disable HUD Canvases
        vrHUD.enabled = b;
        crossHairTarget.enabled = b;

        //Change camera render distance
        if (b){
            cam.Head.camera.farClipPlane = 5020f;
            //otherCam.farClipPlane = 5020f;
        } else {
            cam.Head.camera.farClipPlane = 500f;
            //otherCam.farClipPlane = 500f;
        }
    }

    // Convert text to cm
    public void UpdateHeight(float height)
    {
        heightText.text = Mathf.Round((height * 100f)).ToString();
    }
}
