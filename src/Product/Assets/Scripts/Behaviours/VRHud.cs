using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRHud : MonoBehaviour {
    public Text heightText;
    public Calibration calibration;
    public Canvas vrHud;
    public Canvas crosshairTarget;
    public SteamVR_Camera cam;

    void Start() {
        calibration.ToggleMode += SetEnabled;
        vrHud.enabled = true;
        crosshairTarget.enabled = true;
        cam.camera.farClipPlane = 5020f;
    }

    private void SetEnabled(bool b) {
        // Enable/disable HUD Canvases
        vrHud.enabled = !b;
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
